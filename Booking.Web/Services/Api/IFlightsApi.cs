using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary>
    /// Booking.com API'sinin <c>/api/v1/flights/*</c> uç noktalarının tamamını kapsar.
    /// </summary>
    public interface IFlightsApi
    {
        /// <summary>Havalimanı/şehir arar (fromId/toId için gereklidir).</summary>
        Task<JsonElement> SearchDestinationAsync(string query, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Tek yön / gidiş-dönüş uçuş araması yapar.</summary>
        Task<JsonElement> SearchFlightsAsync(string fromId, string toId, string departDate, string? returnDate = null,
            int adults = 1, string? children = null, string cabinClass = "ECONOMY", string sort = "BEST",
            int pageNo = 1, string? currencyCode = null, CancellationToken ct = default);

        /// <summary>Çok duraklı (multi-stop) uçuş araması yapar.</summary>
        Task<JsonElement> SearchFlightsMultiStopsAsync(string legs, int adults = 1, string? children = null,
            string cabinClass = "ECONOMY", string sort = "BEST", int pageNo = 1, string? currencyCode = null, CancellationToken ct = default);

        /// <summary>Seçilen uçuşun detaylarını getirir.</summary>
        Task<JsonElement> GetFlightDetailsAsync(string token, string? currencyCode = null, CancellationToken ct = default);

        /// <summary>Bir rota için en düşük fiyatı getirir.</summary>
        Task<JsonElement> GetMinPriceAsync(string fromId, string toId, string departDate, string? returnDate = null,
            string cabinClass = "ECONOMY", string? currencyCode = null, CancellationToken ct = default);

        /// <summary>Çok duraklı rota için en düşük fiyatı getirir.</summary>
        Task<JsonElement> GetMinPriceMultiStopsAsync(string legs, string cabinClass = "ECONOMY", string? currencyCode = null, CancellationToken ct = default);

        /// <summary>Seçilen uçuşun koltuk haritasını getirir.</summary>
        Task<JsonElement> GetSeatMapAsync(string offerToken, string? currencyCode = null, CancellationToken ct = default);
    }
}
