using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary><see cref="ITaxiApi"/> uygulaması (<c>/api/v1/taxi/*</c>).</summary>
    public class TaxiApi : ITaxiApi
    {
        private const string Prefix = "/api/v1/taxi";
        private readonly BookingApiClient _client;

        public TaxiApi(BookingApiClient client) => _client = client;

        private string Lang(string? languageCode) =>
            string.IsNullOrWhiteSpace(languageCode) ? _client.DefaultLanguageCode : languageCode!;

        public Task<JsonElement> SearchLocationAsync(string query, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchLocation", new Dictionary<string, string?>
            {
                ["query"] = query,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> SearchTaxiAsync(string pickUpPlaceId, string dropOffPlaceId, string pickUpDate,
            string pickUpTime = "10:00", string? currencyCode = null, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchTaxi", new Dictionary<string, string?>
            {
                ["pick_up_place_id"] = pickUpPlaceId,
                ["drop_off_place_id"] = dropOffPlaceId,
                ["pick_up_date"] = pickUpDate,
                ["pick_up_time"] = pickUpTime,
                ["currency_code"] = currencyCode,
                ["languagecode"] = Lang(languageCode)
            }, ct);
    }
}
