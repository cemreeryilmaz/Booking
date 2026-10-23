using System.Text;
using System.Text.Json;
using Booking.Web.DTOs.Booking;
using Booking.Web.Models;
using Booking.Web.Options;
using Booking.Web.Services.Api;
using Microsoft.Extensions.Options;

namespace Booking.Web.Services
{
    /// <summary>
    /// RapidAPI üzerindeki Booking.com (booking-com15) API'sine bağlanan asıl servis.
    /// 3 temel uç noktayı (lokasyon, otel arama, otel detay) sırasıyla kullanır.
    /// Hata/boş yanıt durumunda arayüz bozulmasın diye mock veriye düşebilir.
    /// </summary>
    public class BookingApiService : IBookingApiService
    {
        private readonly HttpClient _httpClient;
        private readonly BookingApiOptions _options;
        private readonly ILogger<BookingApiService> _logger;
        private readonly IHotelsApi _hotelsApi;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public BookingApiService(HttpClient httpClient, IOptions<BookingApiOptions> options,
            ILogger<BookingApiService> logger, IHotelsApi hotelsApi)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
            _hotelsApi = hotelsApi;
        }

        // ------------------------------------------------------------------
        //  ARAMA + LİSTELEME (1. ve 2. adım)
        // ------------------------------------------------------------------
        public async Task<HotelListViewModel> SearchHotelsAsync(HotelSearchForm form)
        {
            NormalizeDates(form);

            if (_options.UseMockData)
                return MockDataProvider.GetHotelList(form);

            if (string.IsNullOrWhiteSpace(form.Destination))
                return new HotelListViewModel { Search = form, ErrorMessage = "Lütfen bir şehir girin." };

            try
            {
                // 1. ADIM: Şehir adından dest_id bul
                var location = await SearchDestinationAsync(form.Destination!);
                if (location is null || string.IsNullOrWhiteSpace(location.DestId))
                {
                    return new HotelListViewModel
                    {
                        Search = form,
                        ErrorMessage = $"\u201c{form.Destination}\u201d için bir lokasyon bulunamadı. " +
                                       "Lütfen şehir adını kontrol edip tekrar deneyin."
                    };
                }

                // 2. ADIM: dest_id ve form parametreleri ile otelleri getir
                var hotels = await SearchHotelsInternalAsync(location, form);

                var result = new HotelListViewModel
                {
                    Search = form,
                    DestinationLabel = location.Label ?? location.Name ?? form.Destination,
                    Hotels = hotels
                };

                if (!result.HasResults)
                    result.ErrorMessage = "Seçtiğiniz kriterlere uygun otel bulunamadı. " +
                                          "Tarihleri veya misafir sayısını değiştirmeyi deneyin.";

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Otel araması sırasında hata oluştu. Destination: {Destination}", form.Destination);
                return HandleListError(form);
            }
        }

        private async Task<LocationDto?> SearchDestinationAsync(string query)
        {
            var url = $"{_options.BaseUrl}/api/v1/hotels/searchDestination?query={Uri.EscapeDataString(query)}";
            var response = await SendAsync(url);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<LocationSearchResponse>(body, JsonOptions);

            if (parsed?.Data is null || parsed.Data.Count == 0)
                return null;

            // Öncelik şehir (city) tipinde bir sonuçta; yoksa ilk sonuç.
            return parsed.Data.FirstOrDefault(d =>
                       string.Equals(d.SearchType, "city", StringComparison.OrdinalIgnoreCase))
                   ?? parsed.Data.First();
        }

        private async Task<List<HotelCardViewModel>> SearchHotelsInternalAsync(LocationDto location, HotelSearchForm form)
        {
            var searchType = string.IsNullOrWhiteSpace(location.SearchType)
                ? "CITY"
                : location.SearchType!.ToUpperInvariant();

            var url = $"{_options.BaseUrl}/api/v1/hotels/searchHotels" +
                      $"?dest_id={Uri.EscapeDataString(location.DestId!)}" +
                      $"&search_type={searchType}" +
                      $"&arrival_date={form.CheckIn}" +
                      $"&departure_date={form.CheckOut}" +
                      $"&adults={form.Adults}" +
                      $"&room_qty={form.Rooms}" +
                      $"&page_number=1&units=metric&temperature_unit=c" +
                      $"&languagecode={_options.LanguageCode}" +
                      $"&currency_code={form.Currency}";

            if (form.Children > 0 && !string.IsNullOrWhiteSpace(form.ChildAges))
                url += $"&children_age={Uri.EscapeDataString(form.ChildAges!)}";

            var response = await SendAsync(url);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<HotelSearchResponse>(body, JsonOptions);

            var list = new List<HotelCardViewModel>();
            if (parsed?.Data?.Hotels is null)
                return list;

            foreach (var h in parsed.Data.Hotels)
            {
                var p = h.Property;
                if (p is null)
                    continue;

                list.Add(new HotelCardViewModel
                {
                    HotelId = h.HotelId,
                    Name = p.Name,
                    PhotoUrl = p.PhotoUrls?.FirstOrDefault(),
                    City = location.CityName ?? location.Name,
                    Country = location.Country,
                    Price = p.PriceBreakdown?.GrossPrice?.Value,
                    Currency = p.PriceBreakdown?.GrossPrice?.Currency ?? form.Currency,
                    ReviewScore = p.ReviewScore,
                    ReviewScoreWord = p.ReviewScoreWord,
                    ReviewCount = p.ReviewCount,
                    StarRating = p.AccuratePropertyClass ?? p.PropertyClass ?? 0
                });
            }

            return list;
        }

        // ------------------------------------------------------------------
        //  OTEL DETAY (3. adım)
        // ------------------------------------------------------------------
        public async Task<HotelDetailViewModel> GetHotelDetailAsync(long hotelId, HotelSearchForm form)
        {
            NormalizeDates(form);

            if (_options.UseMockData)
                return MockDataProvider.GetHotelDetail(hotelId, form);

            try
            {
                var url = $"{_options.BaseUrl}/api/v1/hotels/getHotelDetails" +
                          $"?hotel_id={hotelId}" +
                          $"&arrival_date={form.CheckIn}" +
                          $"&departure_date={form.CheckOut}" +
                          $"&adults={form.Adults}" +
                          $"&room_qty={form.Rooms}" +
                          $"&units=metric&temperature_unit=c" +
                          $"&languagecode={_options.LanguageCode}" +
                          $"&currency_code={form.Currency}";

                if (form.Children > 0 && !string.IsNullOrWhiteSpace(form.ChildAges))
                    url += $"&children_age={Uri.EscapeDataString(form.ChildAges!)}";

                var response = await SendAsync(url);
                response.EnsureSuccessStatusCode();

                var body = await response.Content.ReadAsStringAsync();
                var parsed = JsonSerializer.Deserialize<HotelDetailsResponse>(body, JsonOptions);

                if (parsed?.Data is null)
                    return HandleDetailError(hotelId, form);

                var detail = MapDetail(parsed.Data, form);
                await EnrichDescriptionAsync(detail);
                return detail;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Otel detayı alınırken hata oluştu. HotelId: {HotelId}", hotelId);
                return HandleDetailError(hotelId, form);
            }
        }

        private HotelDetailViewModel MapDetail(HotelDetailsData data, HotelSearchForm form)
        {
            var vm = new HotelDetailViewModel
            {
                Search = form,
                HotelId = data.HotelId,
                Name = data.HotelName,
                StarRating = data.PropertyClass ?? 0,
                Address = data.Address,
                City = data.City,
                Country = data.Country,
                Latitude = data.Latitude,
                Longitude = data.Longitude,
                ReviewScore = data.ReviewScore,
                ReviewScoreWord = data.ReviewScoreWord,
                ReviewCount = data.ReviewNr,
                Currency = form.Currency
            };

            // Öne çıkanlar
            if (data.PropertyHighlightStrip is not null)
                vm.Highlights = data.PropertyHighlightStrip
                    .Where(h => !string.IsNullOrWhiteSpace(h.Name))
                    .Select(h => h.Name!)
                    .ToList();

            // İmkanlar
            if (data.FacilitiesBlock?.Facilities is not null)
                vm.Facilities = data.FacilitiesBlock.Facilities
                    .Where(f => !string.IsNullOrWhiteSpace(f.Name))
                    .Select(f => new FacilityViewModel { Name = f.Name, Icon = GuessIcon(f.Name!) })
                    .ToList();

            // Fotoğraflar (oda fotoğraflarından toplanır)
            if (data.Rooms is not null)
                vm.PhotoUrls = data.Rooms.Values
                    .Where(r => r.Photos is not null)
                    .SelectMany(r => r.Photos!)
                    .Select(p => p.BestUrl)
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .Select(u => u!)
                    .Distinct()
                    .Take(12)
                    .ToList();

            // Odalar (satın alınabilir teklifler)
            if (data.Block is not null)
            {
                foreach (var b in data.Block)
                {
                    string? photo = null;
                    if (data.Rooms is not null &&
                        data.Rooms.TryGetValue(b.RoomId.ToString(), out var roomDetail))
                        photo = roomDetail.Photos?.FirstOrDefault()?.BestUrl;

                    vm.Rooms.Add(new RoomViewModel
                    {
                        RoomId = b.RoomId,
                        Name = b.NameWithoutPolicy ?? b.Name,
                        MaxOccupancy = b.MaxOccupancy,
                        Price = b.ProductPriceBreakdown?.GrossAmount?.Value,
                        Currency = b.ProductPriceBreakdown?.GrossAmount?.Currency ?? form.Currency,
                        PhotoUrl = photo
                    });
                }
            }

            // Fiyat
            vm.Price = data.CompositePriceBreakdown?.GrossAmount?.Value
                       ?? vm.Rooms.Where(r => r.Price.HasValue).Select(r => r.Price).Min();

            return vm;
        }

        /// <summary>
        /// Otel detayında açıklama boşsa <c>getDescriptionAndInfo</c> uç noktasıyla metni doldurur.
        /// En iyi çaba (best-effort): hata/boş yanıt durumunda sessizce atlar, sayfayı bozmaz.
        /// </summary>
        private async Task EnrichDescriptionAsync(HotelDetailViewModel detail)
        {
            if (!_options.EnrichDetailWithDescription || !string.IsNullOrWhiteSpace(detail.Description))
                return;

            try
            {
                var root = await _hotelsApi.GetDescriptionAndInfoAsync(detail.HotelId);
                detail.Description = ExtractDescription(root);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Otel açıklaması alınamadı (atlanıyor). HotelId: {HotelId}", detail.HotelId);
            }
        }

        /// <summary>getDescriptionAndInfo yanıtındaki açıklama bloklarını tek bir metinde birleştirir.</summary>
        private static string? ExtractDescription(JsonElement root)
        {
            if (!root.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Array)
                return null;

            var sb = new StringBuilder();
            foreach (var block in data.EnumerateArray())
            {
                if (block.TryGetProperty("description", out var desc) && desc.ValueKind == JsonValueKind.String)
                {
                    var text = desc.GetString();
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        if (sb.Length > 0)
                            sb.Append("\n\n");
                        sb.Append(text!.Trim());
                    }
                }
            }

            return sb.Length > 0 ? sb.ToString() : null;
        }

        // ------------------------------------------------------------------
        //  Yardımcılar
        // ------------------------------------------------------------------
        private Task<HttpResponseMessage> SendAsync(string url)
        {
            // Kullanıcının RapidAPI projesindeki ile aynı desen: HttpRequestMessage + header'lar
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers =
                {
                    { "x-rapidapi-key", _options.ApiKey },
                    { "x-rapidapi-host", _options.ApiHost },
                }
            };
            return _httpClient.SendAsync(request);
        }

        /// <summary>Giriş/çıkış tarihleri boşsa makul varsayılanlar atar (API bu alanları zorunlu ister).</summary>
        private static void NormalizeDates(HotelSearchForm form)
        {
            if (!DateTime.TryParse(form.CheckIn, out _))
                form.CheckIn = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");
            if (!DateTime.TryParse(form.CheckOut, out _))
                form.CheckOut = DateTime.Today.AddDays(17).ToString("yyyy-MM-dd");
        }

        private HotelListViewModel HandleListError(HotelSearchForm form)
        {
            if (_options.FallbackToMockOnError)
            {
                var mock = MockDataProvider.GetHotelList(form);
                mock.ErrorMessage = "Canlı API'ye şu an ulaşılamadı; örnek (demo) veriler gösteriliyor.";
                return mock;
            }
            return new HotelListViewModel
            {
                Search = form,
                ErrorMessage = "Oteller yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            };
        }

        private HotelDetailViewModel HandleDetailError(long hotelId, HotelSearchForm form)
        {
            if (_options.FallbackToMockOnError)
            {
                var mock = MockDataProvider.GetHotelDetail(hotelId, form);
                mock.ErrorMessage = "Canlı API'ye şu an ulaşılamadı; örnek (demo) veriler gösteriliyor.";
                return mock;
            }
            return new HotelDetailViewModel
            {
                Search = form,
                HotelId = hotelId,
                ErrorMessage = "Otel detayları yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin."
            };
        }

        /// <summary>İmkan adından uygun bir Bootstrap Icon tahmin eder (sadece görsel amaçlı).</summary>
        private static string GuessIcon(string name)
        {
            var n = name.ToLowerInvariant();
            if (n.Contains("wifi") || n.Contains("internet")) return "bi-wifi";
            if (n.Contains("breakfast") || n.Contains("kahvalt")) return "bi-cup-hot";
            if (n.Contains("park") || n.Contains("otopark")) return "bi-p-circle";
            if (n.Contains("pool") || n.Contains("havuz")) return "bi-water";
            if (n.Contains("spa")) return "bi-flower1";
            if (n.Contains("fitness") || n.Contains("gym")) return "bi-heart-pulse";
            if (n.Contains("air") || n.Contains("klima") || n.Contains("conditioning")) return "bi-snow";
            if (n.Contains("restaurant") || n.Contains("restoran")) return "bi-egg-fried";
            if (n.Contains("bar")) return "bi-cup-straw";
            if (n.Contains("pet") || n.Contains("evcil")) return "bi-emoji-smile";
            if (n.Contains("reception") || n.Contains("resepsiyon") || n.Contains("24")) return "bi-clock";
            if (n.Contains("room service") || n.Contains("oda servis")) return "bi-bell";
            if (n.Contains("bagaj") || n.Contains("luggage")) return "bi-suitcase2";
            return "bi-check2-circle";
        }
    }
}
