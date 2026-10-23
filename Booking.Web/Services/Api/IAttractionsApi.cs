using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary>Booking.com API'sinin <c>/api/v1/attraction/*</c> uç noktalarını kapsar.</summary>
    public interface IAttractionsApi
    {
        /// <summary>Gezilecek yer araması için lokasyon (id) arar.</summary>
        Task<JsonElement> SearchLocationAsync(string query, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Bir lokasyondaki gezilecek yerleri listeler.</summary>
        Task<JsonElement> SearchAttractionsAsync(string id, string? startDate = null, string? endDate = null,
            string sortBy = "trending", int page = 1, string? currencyCode = null, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Bir gezilecek yerin detaylarını getirir.</summary>
        Task<JsonElement> GetAttractionDetailsAsync(string slug, string? currencyCode = null, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Bir gezilecek yer için müsaitlik takvimini getirir.</summary>
        Task<JsonElement> GetAvailabilityCalendarAsync(string id, CancellationToken ct = default);

        /// <summary>Belirli bir tarih için müsaitlik bilgisini getirir.</summary>
        Task<JsonElement> GetAvailabilityAsync(string id, string date, string? currencyCode = null, string? languageCode = null, CancellationToken ct = default);
    }
}
