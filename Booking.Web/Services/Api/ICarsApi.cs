using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary>
    /// Booking.com API'sinin araç kiralama uç noktalarını kapsar:
    /// eski <c>/api/v1/cars/*</c> (deprecated) ve yeni <c>/api/v2/cars/*</c>.
    /// </summary>
    public interface ICarsApi
    {
        // ---- v1 (deprecated) ----
        Task<JsonElement> SearchDestinationAsync(string query, CancellationToken ct = default);
        Task<JsonElement> SearchCarRentalsAsync(double pickUpLatitude, double pickUpLongitude,
            double dropOffLatitude, double dropOffLongitude, string pickUpDate, string dropOffDate,
            string pickUpTime = "10:00", string dropOffTime = "10:00", int driverAge = 30,
            string? currencyCode = null, CancellationToken ct = default);
        Task<JsonElement> GetPackagesAsync(string searchKey, string vehicleId, CancellationToken ct = default);
        Task<JsonElement> VehicleDetailsAsync(string vehicleId, string searchKey, string? currencyCode = null, CancellationToken ct = default);
        Task<JsonElement> BookingSummaryAsync(string vehicleId, string searchKey, string? currencyCode = null, CancellationToken ct = default);
        Task<JsonElement> VehicleSupplierDetailsAsync(string vehicleId, string searchKey, CancellationToken ct = default);
        Task<JsonElement> VehicleSupplierRatingsAsync(string vehicleId, string searchKey, CancellationToken ct = default);
        Task<JsonElement> VehicleSupplierReviewAsync(string vehicleId, string searchKey, CancellationToken ct = default);

        // ---- v2 ----
        Task<JsonElement> SearchDestinationV2Async(string query, CancellationToken ct = default);
        Task<JsonElement> SearchCarRentalsV2Async(double pickUpLatitude, double pickUpLongitude,
            double dropOffLatitude, double dropOffLongitude, string pickUpDate, string dropOffDate,
            string pickUpTime = "10:00", string dropOffTime = "10:00", int driverAge = 30,
            string? currencyCode = null, CancellationToken ct = default);
        Task<JsonElement> VehicleDetailsV2Async(string vehicleId, string searchKey, string? currencyCode = null, CancellationToken ct = default);
    }
}
