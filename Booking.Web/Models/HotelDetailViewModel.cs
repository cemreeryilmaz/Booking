namespace Booking.Web.Models
{
    /// <summary>Otel detay sayfasının tüm verisi.</summary>
    public class HotelDetailViewModel
    {
        public HotelSearchForm Search { get; set; } = new();

        public long HotelId { get; set; }
        public string? Name { get; set; }
        public int StarRating { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public double? ReviewScore { get; set; }
        public string? ReviewScoreWord { get; set; }
        public int? ReviewCount { get; set; }

        public string? Description { get; set; }

        public List<string> Highlights { get; set; } = new();
        public List<FacilityViewModel> Facilities { get; set; } = new();
        public List<string> PhotoUrls { get; set; } = new();
        public List<RoomViewModel> Rooms { get; set; } = new();

        public double? Price { get; set; }
        public string Currency { get; set; } = "EUR";

        public string? ErrorMessage { get; set; }
        public bool IsMockData { get; set; }

        public string CurrencySymbol => CurrencyHelper.ToSymbol(Currency);
        public string ReviewScoreText =>
            ReviewScore.HasValue ? ReviewScore.Value.ToString("0.0") : "-";
        public string PriceText =>
            Price.HasValue ? $"{CurrencySymbol}{Price.Value:N0}" : "-";

        public string FullAddress =>
            string.Join(", ", new[] { Address, City, Country }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    public class FacilityViewModel
    {
        public string? Name { get; set; }
        /// <summary>Bootstrap Icons sınıf adı (örn: "bi-wifi").</summary>
        public string Icon { get; set; } = "bi-check2-circle";
    }

    public class RoomViewModel
    {
        public long RoomId { get; set; }
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public string? MaxOccupancy { get; set; }
        public double? Price { get; set; }
        public string Currency { get; set; } = "EUR";

        public string CurrencySymbol => CurrencyHelper.ToSymbol(Currency);
        public string PriceText =>
            Price.HasValue ? $"{CurrencySymbol}{Price.Value:N0}" : "-";
    }
}
