using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary><see cref="IFlightsApi"/> uygulaması (<c>/api/v1/flights/*</c>).</summary>
    public class FlightsApi : IFlightsApi
    {
        private const string Prefix = "/api/v1/flights";
        private readonly BookingApiClient _client;

        public FlightsApi(BookingApiClient client) => _client = client;

        private string Lang(string? languageCode) =>
            string.IsNullOrWhiteSpace(languageCode) ? _client.DefaultLanguageCode : languageCode!;

        public Task<JsonElement> SearchDestinationAsync(string query, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchDestination", new Dictionary<string, string?>
            {
                ["query"] = query,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> SearchFlightsAsync(string fromId, string toId, string departDate, string? returnDate = null,
            int adults = 1, string? children = null, string cabinClass = "ECONOMY", string sort = "BEST",
            int pageNo = 1, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchFlights", new Dictionary<string, string?>
            {
                ["fromId"] = fromId,
                ["toId"] = toId,
                ["departDate"] = departDate,
                ["returnDate"] = returnDate,
                ["adults"] = adults.ToString(),
                ["children"] = children,
                ["cabinClass"] = cabinClass,
                ["sort"] = sort,
                ["pageNo"] = pageNo.ToString(),
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> SearchFlightsMultiStopsAsync(string legs, int adults = 1, string? children = null,
            string cabinClass = "ECONOMY", string sort = "BEST", int pageNo = 1, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchFlightsMultiStops", new Dictionary<string, string?>
            {
                ["legs"] = legs,
                ["adults"] = adults.ToString(),
                ["children"] = children,
                ["cabinClass"] = cabinClass,
                ["sort"] = sort,
                ["pageNo"] = pageNo.ToString(),
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> GetFlightDetailsAsync(string token, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getFlightDetails", new Dictionary<string, string?>
            {
                ["token"] = token,
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> GetMinPriceAsync(string fromId, string toId, string departDate, string? returnDate = null,
            string cabinClass = "ECONOMY", string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getMinPrice", new Dictionary<string, string?>
            {
                ["fromId"] = fromId,
                ["toId"] = toId,
                ["departDate"] = departDate,
                ["returnDate"] = returnDate,
                ["cabinClass"] = cabinClass,
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> GetMinPriceMultiStopsAsync(string legs, string cabinClass = "ECONOMY", string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getMinPriceMultiStops", new Dictionary<string, string?>
            {
                ["legs"] = legs,
                ["cabinClass"] = cabinClass,
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> GetSeatMapAsync(string offerToken, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getSeatMap", new Dictionary<string, string?>
            {
                ["offerToken"] = offerToken,
                ["currency_code"] = currencyCode
            }, ct);
    }
}
