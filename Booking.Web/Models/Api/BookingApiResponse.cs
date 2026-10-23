using System.Text.Json;
using System.Text.Json.Serialization;

namespace Booking.Web.Models.Api
{
    /// <summary>
    /// Booking.com (booking-com15) API'sinin tüm uç noktalarında kullandığı ortak yanıt zarfı:
    /// <c>{ status, message, timestamp, data }</c>.
    /// <para>
    /// <c>message</c> bazı uç noktalarda düz metin, bazılarında dizi/nesne dönebildiği için
    /// esnek olması adına <see cref="JsonElement"/> olarak tutulur.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Beklenen <c>data</c> gövdesinin tipi.</typeparam>
    public class BookingApiResponse<T>
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public JsonElement Message { get; set; }

        [JsonPropertyName("timestamp")]
        public long? Timestamp { get; set; }

        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
