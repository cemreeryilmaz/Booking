using System.Text;
using System.Text.Json;
using Booking.Web.Models.Api;
using Booking.Web.Options;
using Microsoft.Extensions.Options;

namespace Booking.Web.Services.Api
{
    /// <summary>
    /// Tüm Booking.com (booking-com15) uç noktaları için ortak, düşük seviyeli HTTP istemcisi.
    /// Kategori servisleri (Hotels, Flights, Cars, Taxi, Attraction, Meta) yalnızca bu istemciyi
    /// kullanır; böylece header, URL kurulumu ve JSON çözümleme tek yerde toplanır.
    /// </summary>
    public class BookingApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly BookingApiOptions _options;
        private readonly ILogger<BookingApiClient> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BookingApiClient(HttpClient httpClient, IOptions<BookingApiOptions> options,
            ILogger<BookingApiClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>Yapılandırmadaki varsayılan dil kodu (çağrılar boş geçtiğinde kullanılır).</summary>
        public string DefaultLanguageCode => _options.LanguageCode;

        /// <summary>Ham JSON yanıtını <see cref="JsonElement"/> olarak döndürür.</summary>
        public async Task<JsonElement> GetRawAsync(string path,
            IReadOnlyDictionary<string, string?>? query = null, CancellationToken ct = default)
        {
            using var response = await SendAsync(path, query, ct);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(body);
            return doc.RootElement.Clone();
        }

        /// <summary>JSON yanıtını verilen tipe çözümler.</summary>
        public async Task<T?> GetAsync<T>(string path,
            IReadOnlyDictionary<string, string?>? query = null, CancellationToken ct = default)
        {
            using var response = await SendAsync(path, query, ct);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<T>(body, JsonOptions);
        }

        /// <summary>JSON yanıtını ortak <see cref="BookingApiResponse{T}"/> zarfıyla çözümler.</summary>
        public Task<BookingApiResponse<T>?> GetResponseAsync<T>(string path,
            IReadOnlyDictionary<string, string?>? query = null, CancellationToken ct = default)
            => GetAsync<BookingApiResponse<T>>(path, query, ct);

        private Task<HttpResponseMessage> SendAsync(string path,
            IReadOnlyDictionary<string, string?>? query, CancellationToken ct)
        {
            var url = BuildUrl(path, query);

            // Kullanıcının RapidAPI projesindeki ile aynı desen: HttpRequestMessage + header'lar.
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
            request.Headers.Add("x-rapidapi-key", _options.ApiKey);
            request.Headers.Add("x-rapidapi-host", _options.ApiHost);

            _logger.LogDebug("Booking API çağrısı: {Url}", url);
            return _httpClient.SendAsync(request, ct);
        }

        /// <summary>Taban adres + yol + boş olmayan sorgu parametrelerinden tam URL kurar.</summary>
        private string BuildUrl(string path, IReadOnlyDictionary<string, string?>? query)
        {
            var sb = new StringBuilder(_options.BaseUrl.TrimEnd('/'));
            if (!path.StartsWith('/'))
                sb.Append('/');
            sb.Append(path);

            if (query is { Count: > 0 })
            {
                var first = true;
                foreach (var kv in query)
                {
                    if (string.IsNullOrWhiteSpace(kv.Value))
                        continue;

                    sb.Append(first ? '?' : '&');
                    sb.Append(Uri.EscapeDataString(kv.Key));
                    sb.Append('=');
                    sb.Append(Uri.EscapeDataString(kv.Value));
                    first = false;
                }
            }

            return sb.ToString();
        }
    }
}
