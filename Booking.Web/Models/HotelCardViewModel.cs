namespace Booking.Web.Models
{
    /// <summary>Listeleme sayfasındaki tek bir otel kartını temsil eder.</summary>
    public class HotelCardViewModel
    {
        public long HotelId { get; set; }
        public string? Name { get; set; }
        public string? PhotoUrl { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public double? Price { get; set; }
        public string Currency { get; set; } = "EUR";

        public double? ReviewScore { get; set; }
        public string? ReviewScoreWord { get; set; }
        public int? ReviewCount { get; set; }

        public int StarRating { get; set; }

        /// <summary>Puanı "8,7" gibi Türkçe formatta gösterir.</summary>
        public string ReviewScoreText =>
            ReviewScore.HasValue ? ReviewScore.Value.ToString("0.0") : "-";

        public string PriceText =>
            Price.HasValue ? $"{CurrencySymbol}{Price.Value:N0}" : "-";

        public string CurrencySymbol => CurrencyHelper.ToSymbol(Currency);
    }

    /// <summary>Para birimi kodunu sembole çevirir (küçük yardımcı).</summary>
    public static class CurrencyHelper
    {
        public static string ToSymbol(string? currency) => (currency ?? "EUR").ToUpperInvariant() switch
        {
            "EUR" => "€",
            "USD" => "$",
            "GBP" => "£",
            "TRY" => "₺",
            _ => (currency ?? "EUR") + " "
        };
    }
}
