using System.Text.Json;
using Booking.Web.Models.Api;

namespace Booking.Web.Services.Api
{
    /// <summary>
    /// Booking.com API'sinin <c>/api/v1/hotels/*</c> uç noktalarının tamamını kapsar.
    /// Her metot tek bir uç noktaya karşılık gelir ve ham <see cref="JsonElement"/> döndürür.
    /// </summary>
    public interface IHotelsApi
    {
        /// <summary>Şehir/bölge/otel adından lokasyon (dest_id) arar.</summary>
        Task<JsonElement> SearchDestinationAsync(string query, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Arama için kullanılabilir filtre seçeneklerini döndürür.</summary>
        Task<JsonElement> GetFilterAsync(string destId, string searchType, StayParams stay, CancellationToken ct = default);

        /// <summary>Arama için kullanılabilir sıralama seçeneklerini döndürür.</summary>
        Task<JsonElement> GetSortByAsync(string destId, string searchType, StayParams stay, CancellationToken ct = default);

        /// <summary>Lokasyona (dest_id) göre otelleri listeler.</summary>
        Task<JsonElement> SearchHotelsAsync(string destId, string searchType, StayParams stay,
            string? categoriesFilterIds = null, string? sortBy = null, CancellationToken ct = default);

        /// <summary>Koordinata (enlem/boylam) göre otelleri listeler.</summary>
        Task<JsonElement> SearchHotelsByCoordinatesAsync(double latitude, double longitude, StayParams stay, CancellationToken ct = default);

        /// <summary>Tek bir otelin detaylarını getirir.</summary>
        Task<JsonElement> GetHotelDetailsAsync(long hotelId, StayParams stay, CancellationToken ct = default);

        /// <summary>İleri tarihler için oda müsaitliğini sorgular.</summary>
        Task<JsonElement> GetAvailabilityAsync(long hotelId, string? minDate = null, string? maxDate = null,
            string? currencyCode = null, string? location = null, CancellationToken ct = default);

        /// <summary>Otelin açıklama ve tanıtım bilgilerini getirir.</summary>
        Task<JsonElement> GetDescriptionAndInfoAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otelin oda listesini getirir.</summary>
        Task<JsonElement> GetRoomListAsync(long hotelId, StayParams stay, CancellationToken ct = default);

        /// <summary>Otelin müsaitlik bilgisiyle birlikte oda listesini getirir.</summary>
        Task<JsonElement> GetRoomListWithAvailabilityAsync(long hotelId, StayParams stay, CancellationToken ct = default);

        /// <summary>Otelin ödeme özelliklerini getirir.</summary>
        Task<JsonElement> GetPaymentFeaturesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otel politikalarını getirir.</summary>
        Task<JsonElement> GetHotelPoliciesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otelin çocuk politikalarını getirir.</summary>
        Task<JsonElement> GetPropertyChildrenPoliciesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otel yorumları için filtre meta verisini getirir.</summary>
        Task<JsonElement> GetHotelReviewsFilterMetadataAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otelin değerlendirme puanlarını (kategori bazlı) getirir.</summary>
        Task<JsonElement> GetHotelReviewScoresAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otel yorumları için mevcut sıralama seçeneklerini getirir.</summary>
        Task<JsonElement> GetHotelReviewsSortOptionAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otel yorumlarını (ipuçları) getirir.</summary>
        Task<JsonElement> GetHotelReviewsAsync(long hotelId, int pageNumber = 1, string? sortOption = null, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otele dair soru-cevapları getirir.</summary>
        Task<JsonElement> GetQuestionAndAnswerAsync(long hotelId, int pageNumber = 1, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Koordinata yakın şehirleri getirir (deprecated).</summary>
        Task<JsonElement> GetNearbyCitiesAsync(double latitude, double longitude, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otel çevresindeki ilgi noktalarını (restoran, ulaşım, plaj vb.) getirir.</summary>
        Task<JsonElement> GetPropertySurroundingsAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otele yakın popüler gezilecek yerleri getirir.</summary>
        Task<JsonElement> GetPopularAttractionNearByAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otelin fotoğraflarını getirir.</summary>
        Task<JsonElement> GetHotelPhotosAsync(long hotelId, CancellationToken ct = default);

        /// <summary>Otelin sunduğu imkanları (facilities) getirir.</summary>
        Task<JsonElement> GetHotelFacilitiesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default);

        /// <summary>Otelin giriş/çıkış saatlerini ve ödeme yöntemlerini getirir.</summary>
        Task<JsonElement> GetHotelCheckInOutTimeAsync(long hotelId, string arrivalDate, string departureDate, string? languageCode = null, CancellationToken ct = default);
    }
}
