using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary><see cref="IAttractionsApi"/> uygulaması (<c>/api/v1/attraction/*</c>).</summary>
    public class AttractionsApi : IAttractionsApi
    {
        private const string Prefix = "/api/v1/attraction";
        private readonly BookingApiClient _client;

        public AttractionsApi(BookingApiClient client) => _client = client;

        private string Lang(string? languageCode) =>
            string.IsNullOrWhiteSpace(languageCode) ? _client.DefaultLanguageCode : languageCode!;

        public Task<JsonElement> SearchLocationAsync(string query, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchLocation", new Dictionary<string, string?>
            {
                ["query"] = query,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> SearchAttractionsAsync(string id, string? startDate = null, string? endDate = null,
            string sortBy = "trending", int page = 1, string? currencyCode = null, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchAttractions", new Dictionary<string, string?>
            {
                ["id"] = id,
                ["startDate"] = startDate,
                ["endDate"] = endDate,
                ["sortBy"] = sortBy,
                ["page"] = page.ToString(),
                ["currency_code"] = currencyCode,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetAttractionDetailsAsync(string slug, string? currencyCode = null, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getAttractionDetails", new Dictionary<string, string?>
            {
                ["slug"] = slug,
                ["currency_code"] = currencyCode,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetAvailabilityCalendarAsync(string id, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getAvailabilityCalendar", new Dictionary<string, string?>
            {
                ["id"] = id
            }, ct);

        public Task<JsonElement> GetAvailabilityAsync(string id, string date, string? currencyCode = null, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getAvailability", new Dictionary<string, string?>
            {
                ["id"] = id,
                ["date"] = date,
                ["currency_code"] = currencyCode,
                ["languagecode"] = Lang(languageCode)
            }, ct);
    }
}
