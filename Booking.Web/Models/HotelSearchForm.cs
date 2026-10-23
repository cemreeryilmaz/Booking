namespace Booking.Web.Models
{
    /// <summary>
    /// Arama formundan gelen parametreler. Hem listeleme hem detay akışında
    /// taşınır ki kullanıcı seçtiği tarih / misafir bilgilerini kaybetmesin.
    /// </summary>
    public class HotelSearchForm
    {
        public string? Destination { get; set; }

        // yyyy-MM-dd formatında (HTML <input type="date"> ile uyumlu)
        public string? CheckIn { get; set; }
        public string? CheckOut { get; set; }

        public int Adults { get; set; } = 2;
        public int Children { get; set; } = 0;

        // Çocuk yaşları virgülle ayrılmış olarak gelir (örn: "2,5"). API bu formatı bekler.
        public string? ChildAges { get; set; }

        public int Rooms { get; set; } = 1;
        public string Currency { get; set; } = "EUR";

        public string? SortBy { get; set; }

        /// <summary>Giriş-çıkış tarihleri arasındaki gece sayısı.</summary>
        public int Nights
        {
            get
            {
                if (DateTime.TryParse(CheckIn, out var inDate) &&
                    DateTime.TryParse(CheckOut, out var outDate))
                {
                    var diff = (outDate - inDate).Days;
                    return diff > 0 ? diff : 0;
                }
                return 0;
            }
        }

        /// <summary>"2 Yetişkin, 1 Çocuk" gibi okunabilir misafir metni.</summary>
        public string GuestSummary
        {
            get
            {
                var text = $"{Adults} Yetişkin";
                if (Children > 0)
                    text += $", {Children} Çocuk";
                return text;
            }
        }
    }
}
