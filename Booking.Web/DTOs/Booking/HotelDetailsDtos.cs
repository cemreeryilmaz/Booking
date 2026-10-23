using System.Text.Json.Serialization;

namespace Booking.Web.DTOs.Booking
{
    // 3. ADIM: Otel Detay (getHotelDetails)
    // GET /api/v1/hotels/getHotelDetails?hotel_id={id}&arrival_date=...&departure_date=...
    //         &adults=...&children_age=...&room_qty=...&currency_code=...
    // Seçilen otelin detaylı bilgileri (adres, imkanlar, odalar, fotoğraflar) çekilir.

    public class HotelDetailsResponse
    {
        [JsonPropertyName("status")]
        public bool Status { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public HotelDetailsData? Data { get; set; }
    }

    public class HotelDetailsData
    {
        [JsonPropertyName("hotel_id")]
        public long HotelId { get; set; }

        [JsonPropertyName("hotel_name")]
        public string? HotelName { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("country_trans")]
        public string? Country { get; set; }

        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        [JsonPropertyName("latitude")]
        public double? Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double? Longitude { get; set; }

        [JsonPropertyName("review_nr")]
        public int? ReviewNr { get; set; }

        [JsonPropertyName("review_score")]
        public double? ReviewScore { get; set; }

        [JsonPropertyName("review_score_word")]
        public string? ReviewScoreWord { get; set; }

        [JsonPropertyName("propertyClass")]
        public int? PropertyClass { get; set; }

        [JsonPropertyName("facilities_block")]
        public FacilitiesBlockDto? FacilitiesBlock { get; set; }

        [JsonPropertyName("property_highlight_strip")]
        public List<HighlightDto>? PropertyHighlightStrip { get; set; }

        // Oda fotoğrafları oda kimliğine göre sözlük şeklinde döner.
        [JsonPropertyName("rooms")]
        public Dictionary<string, RoomDetailDto>? Rooms { get; set; }

        // Satın alınabilir oda teklifleri (isim, fiyat, kapasite).
        [JsonPropertyName("block")]
        public List<RoomBlockDto>? Block { get; set; }

        [JsonPropertyName("composite_price_breakdown")]
        public CompositePriceDto? CompositePriceBreakdown { get; set; }
    }

    public class FacilitiesBlockDto
    {
        [JsonPropertyName("facilities")]
        public List<FacilityDto>? Facilities { get; set; }
    }

    public class FacilityDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
    }

    public class HighlightDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class RoomDetailDto
    {
        [JsonPropertyName("photos")]
        public List<RoomPhotoDto>? Photos { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("private_bathroom_count")]
        public int? PrivateBathroomCount { get; set; }
    }

    public class RoomPhotoDto
    {
        [JsonPropertyName("url_max300")]
        public string? UrlMax300 { get; set; }

        [JsonPropertyName("url_original")]
        public string? UrlOriginal { get; set; }

        [JsonPropertyName("url_640x200")]
        public string? Url640x200 { get; set; }

        public string? BestUrl => UrlOriginal ?? UrlMax300 ?? Url640x200;
    }

    public class RoomBlockDto
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("name_without_policy")]
        public string? NameWithoutPolicy { get; set; }

        [JsonPropertyName("max_occupancy")]
        public string? MaxOccupancy { get; set; }

        [JsonPropertyName("product_price_breakdown")]
        public CompositePriceDto? ProductPriceBreakdown { get; set; }
    }

    public class CompositePriceDto
    {
        [JsonPropertyName("gross_amount")]
        public AmountDto? GrossAmount { get; set; }

        [JsonPropertyName("gross_amount_per_night")]
        public AmountDto? GrossAmountPerNight { get; set; }

        [JsonPropertyName("all_inclusive_amount")]
        public AmountDto? AllInclusiveAmount { get; set; }
    }

    public class AmountDto
    {
        [JsonPropertyName("value")]
        public double? Value { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("amount_rounded")]
        public string? AmountRounded { get; set; }
    }
}
