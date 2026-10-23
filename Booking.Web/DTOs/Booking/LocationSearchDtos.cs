using System.Text.Json.Serialization;

namespace Booking.Web.DTOs.Booking
{
    // 1. ADIM: Lokasyon Bulma (searchDestination)
    // GET /api/v1/hotels/searchDestination?query={city}
    // Şehir adından dest_id (eşsiz lokasyon kimliği) elde edilir.

    public class LocationSearchResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public List<LocationDto>? Data { get; set; }
    }

    public class LocationDto
    {
        [JsonPropertyName("dest_id")]
        public string? DestId { get; set; }

        [JsonPropertyName("search_type")]
        public string? SearchType { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("city_name")]
        public string? CityName { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("hotels")]
        public int? Hotels { get; set; }

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("dest_type")]
        public string? DestType { get; set; }
    }
}
