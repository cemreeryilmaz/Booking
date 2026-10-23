using System.Text.Json;

namespace Booking.Web.Services.Api
{
    /// <summary>
    /// Booking.com API'sinin yardımcı uç noktalarını kapsar: <c>/api/v1/meta/*</c> ve <c>/api/v1/test</c>.
    /// </summary>
    public interface IMetaApi
    {
        /// <summary>Desteklenen dillerin listesini getirir.</summary>
        Task<JsonElement> GetLanguagesAsync(CancellationToken ct = default);

        /// <summary>Kullanılabilir para birimlerinin listesini getirir.</summary>
        Task<JsonElement> GetCurrencyAsync(CancellationToken ct = default);

        /// <summary>Taban para birimine göre döviz kurlarını getirir.</summary>
        Task<JsonElement> GetExchangeRatesAsync(string? baseCurrency = null, CancellationToken ct = default);

        /// <summary>Bir adres/konumun enlem-boylamını getirir.</summary>
        Task<JsonElement> LocationToLatLongAsync(string query, CancellationToken ct = default);

        /// <summary>Genel konum araması yapar.</summary>
        Task<JsonElement> GetLocationsAsync(string? name = null, CancellationToken ct = default);

        /// <summary>Sunucunun ayakta olup olmadığını kontrol eder (<c>/api/v1/test</c>).</summary>
        Task<JsonElement> TestAsync(CancellationToken ct = default);
    }
}
