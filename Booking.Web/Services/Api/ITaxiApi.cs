using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary>Booking.com API'sinin <c>/api/v1/taxi/*</c> uç noktalarını kapsar.</summary>
    public interface ITaxiApi
    {
        /// <summary>Taksi için konum (place_id) arar.</summary>
        Task<JsonElement> SearchLocationAsync(string query, string? languageCode = null, CancellationToken ct = default);

        /// <summary>İki nokta arasında taksi/transfer araması yapar.</summary>
        Task<JsonElement> SearchTaxiAsync(string pickUpPlaceId, string dropOffPlaceId, string pickUpDate,
            string pickUpTime = "10:00", string? currencyCode = null, string? languageCode = null, CancellationToken ct = default);
    }
}
