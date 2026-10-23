namespace Booking.Web.Models
{
    /// <summary>Otel listeleme sayfasının tüm verisi.</summary>
    public class HotelListViewModel
    {
        public HotelSearchForm Search { get; set; } = new();

        /// <summary>Lokasyon aramasından dönen okunabilir etiket (örn: "Milan, Lombardy, Italy").</summary>
        public string? DestinationLabel { get; set; }

        public List<HotelCardViewModel> Hotels { get; set; } = new();

        public int ResultCount => Hotels.Count;

        /// <summary>Kullanıcıya gösterilecek hata mesajı (şehir bulunamadı, API hatası vb.).</summary>
        public string? ErrorMessage { get; set; }

        /// <summary>true ise bu veri canlı API yerine mock veriden geldi.</summary>
        public bool IsMockData { get; set; }

        public bool HasResults => Hotels.Count > 0;
    }
}
