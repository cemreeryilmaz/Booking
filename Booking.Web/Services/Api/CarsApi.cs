using System.Globalization;
using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary><see cref="ICarsApi"/> uygulaması (<c>/api/v1/cars/*</c> ve <c>/api/v2/cars/*</c>).</summary>
    public class CarsApi : ICarsApi
    {
        private const string V1 = "/api/v1/cars";
        private const string V2 = "/api/v2/cars";
        private readonly BookingApiClient _client;

        public CarsApi(BookingApiClient client) => _client = client;

        private static string Num(double value) => value.ToString(CultureInfo.InvariantCulture);

        private static Dictionary<string, string?> RentalQuery(double pLat, double pLon, double dLat, double dLon,
            string pDate, string dDate, string pTime, string dTime, int driverAge, string? currencyCode) => new()
        {
            ["pick_up_latitude"] = Num(pLat),
            ["pick_up_longitude"] = Num(pLon),
            ["drop_off_latitude"] = Num(dLat),
            ["drop_off_longitude"] = Num(dLon),
            ["pick_up_date"] = pDate,
            ["drop_off_date"] = dDate,
            ["pick_up_time"] = pTime,
            ["drop_off_time"] = dTime,
            ["driver_age"] = driverAge.ToString(),
            ["currency_code"] = currencyCode
        };

        // ---- v1 ----
        public Task<JsonElement> SearchDestinationAsync(string query, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/searchDestination", new Dictionary<string, string?> { ["query"] = query }, ct);

        public Task<JsonElement> SearchCarRentalsAsync(double pickUpLatitude, double pickUpLongitude,
            double dropOffLatitude, double dropOffLongitude, string pickUpDate, string dropOffDate,
            string pickUpTime = "10:00", string dropOffTime = "10:00", int driverAge = 30,
            string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/searchCarRentals", RentalQuery(pickUpLatitude, pickUpLongitude,
                dropOffLatitude, dropOffLongitude, pickUpDate, dropOffDate, pickUpTime, dropOffTime, driverAge, currencyCode), ct);

        public Task<JsonElement> GetPackagesAsync(string searchKey, string vehicleId, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/getPackages", new Dictionary<string, string?>
            {
                ["search_key"] = searchKey,
                ["vehicle_id"] = vehicleId
            }, ct);

        public Task<JsonElement> VehicleDetailsAsync(string vehicleId, string searchKey, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/vehicleDetails", new Dictionary<string, string?>
            {
                ["vehicle_id"] = vehicleId,
                ["search_key"] = searchKey,
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> BookingSummaryAsync(string vehicleId, string searchKey, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/bookingSummary", new Dictionary<string, string?>
            {
                ["vehicle_id"] = vehicleId,
                ["search_key"] = searchKey,
                ["currency_code"] = currencyCode
            }, ct);

        public Task<JsonElement> VehicleSupplierDetailsAsync(string vehicleId, string searchKey, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/vehicleSupplierDetails", new Dictionary<string, string?>
            {
                ["vehicle_id"] = vehicleId,
                ["search_key"] = searchKey
            }, ct);

        public Task<JsonElement> VehicleSupplierRatingsAsync(string vehicleId, string searchKey, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/vehicleSupplierRatings", new Dictionary<string, string?>
            {
                ["vehicle_id"] = vehicleId,
                ["search_key"] = searchKey
            }, ct);

        public Task<JsonElement> VehicleSupplierReviewAsync(string vehicleId, string searchKey, CancellationToken ct = default)
            => _client.GetRawAsync($"{V1}/vehicleSupplierReview", new Dictionary<string, string?>
            {
                ["vehicle_id"] = vehicleId,
                ["search_key"] = searchKey
            }, ct);

        // ---- v2 ----
        public Task<JsonElement> SearchDestinationV2Async(string query, CancellationToken ct = default)
            => _client.GetRawAsync($"{V2}/searchDestination", new Dictionary<string, string?> { ["query"] = query }, ct);

        public Task<JsonElement> SearchCarRentalsV2Async(double pickUpLatitude, double pickUpLongitude,
            double dropOffLatitude, double dropOffLongitude, string pickUpDate, string dropOffDate,
            string pickUpTime = "10:00", string dropOffTime = "10:00", int driverAge = 30,
            string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{V2}/searchCarRentals", RentalQuery(pickUpLatitude, pickUpLongitude,
                dropOffLatitude, dropOffLongitude, pickUpDate, dropOffDate, pickUpTime, dropOffTime, driverAge, currencyCode), ct);

        public Task<JsonElement> VehicleDetailsV2Async(string vehicleId, string searchKey, string? currencyCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{V2}/vehicleDetails", new Dictionary<string, string?>
            {
                ["vehicle_id"] = vehicleId,
                ["search_key"] = searchKey,
                ["currency_code"] = currencyCode
            }, ct);
    }
}
