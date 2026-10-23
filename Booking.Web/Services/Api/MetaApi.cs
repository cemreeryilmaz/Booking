using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary><see cref="IMetaApi"/> uygulaması (<c>/api/v1/meta/*</c> ve <c>/api/v1/test</c>).</summary>
    public class MetaApi : IMetaApi
    {
        private const string Prefix = "/api/v1/meta";
        private readonly BookingApiClient _client;

        public MetaApi(BookingApiClient client) => _client = client;

        public Task<JsonElement> GetLanguagesAsync(CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getLanguages", null, ct);

        public Task<JsonElement> GetCurrencyAsync(CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getCurrency", null, ct);

        public Task<JsonElement> GetExchangeRatesAsync(string? baseCurrency = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getExchangeRates", new Dictionary<string, string?>
            {
                ["base_currency"] = baseCurrency
            }, ct);

        public Task<JsonElement> LocationToLatLongAsync(string query, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/locationToLatLong", new Dictionary<string, string?>
            {
                ["query"] = query
            }, ct);

        public Task<JsonElement> GetLocationsAsync(string? name = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getLocations", new Dictionary<string, string?>
            {
                ["name"] = name
            }, ct);

        public Task<JsonElement> TestAsync(CancellationToken ct = default)
            => _client.GetRawAsync("/api/v1/test", null, ct);
    }
}
