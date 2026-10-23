using System.Text.Json.Serialization;

namespace Booking.Web.DTOs.Booking
{
    // 2. ADIM: Otel Arama (searchHotels)
    // GET /api/v1/hotels/searchHotels?dest_id={id}&search_type=CITY&arrival_date=...&departure_date=...
    //         &adults=...&children_age=...&room_qty=...&currency_code=...
    // 1. adımdan gelen dest_id ve form parametreleri ile otel listesi çekilir.

    public class HotelSearchResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public HotelSearchData? Data { get; set; }
    }

    public class HotelSearchData
    {
        [JsonPropertyName("hotels")]
        public List<HotelResultDto>? Hotels { get; set; }
    }

    public class HotelResultDto
    {
        [JsonPropertyName("hotel_id")]
        public long HotelId { get; set; }

        [JsonPropertyName("accessibilityLabel")]
        public string? AccessibilityLabel { get; set; }

        [JsonPropertyName("property")]
        public HotelPropertyDto? Property { get; set; }
    }

    public class HotelPropertyDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("reviewScore")]
        public double? ReviewScore { get; set; }

        [JsonPropertyName("reviewCount")]
        public int? ReviewCount { get; set; }

        [JsonPropertyName("reviewScoreWord")]
        public string? ReviewScoreWord { get; set; }

        [JsonPropertyName("propertyClass")]
        public int? PropertyClass { get; set; }

        [JsonPropertyName("accuratePropertyClass")]
        public int? AccuratePropertyClass { get; set; }

        [JsonPropertyName("photoUrls")]
        public List<string>? PhotoUrls { get; set; }

        [JsonPropertyName("priceBreakdown")]
        public PriceBreakdownDto? PriceBreakdown { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("countryCode")]
        public string? CountryCode { get; set; }

        [JsonPropertyName("wishlistName")]
        public string? WishlistName { get; set; }

        [JsonPropertyName("checkinDate")]
        public string? CheckinDate { get; set; }

        [JsonPropertyName("checkoutDate")]
        public string? CheckoutDate { get; set; }
    }

    public class PriceBreakdownDto
    {
        [JsonPropertyName("grossPrice")]
        public MoneyDto? GrossPrice { get; set; }

        [JsonPropertyName("excludedPrice")]
        public MoneyDto? ExcludedPrice { get; set; }
    }

    public class MoneyDto
    {
        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
    }
}
