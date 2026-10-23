namespace Booking.Web.Models.Api
{
    /// <summary>
    /// Konaklama (tarih + misafir + birim) parametreleri. Otel arama, detay, oda listesi,
    /// müsaitlik gibi birden çok uç noktada ortak olarak kullanılır.
    /// </summary>
    public record StayParams
    {
        /// <summary>Giriş tarihi (yyyy-MM-dd).</summary>
        public string? ArrivalDate { get; init; }

        /// <summary>Çıkış tarihi (yyyy-MM-dd).</summary>
        public string? DepartureDate { get; init; }

        /// <summary>Yetişkin sayısı.</summary>
        public int Adults { get; init; } = 1;

        /// <summary>Çocuk yaşları (virgülle ayrılmış, ör. "5,10").</summary>
        public string? ChildrenAge { get; init; }

        /// <summary>Oda sayısı.</summary>
        public int RoomQty { get; init; } = 1;

        /// <summary>Sayfa numarası (listeleme uç noktaları için).</summary>
        public int PageNumber { get; init; } = 1;

        /// <summary>Birim sistemi: metric / imperial.</summary>
        public string Units { get; init; } = "metric";

        /// <summary>Sıcaklık birimi: c / f.</summary>
        public string TemperatureUnit { get; init; } = "c";

        /// <summary>Dil kodu (ör. "en-us"). Boşsa yapılandırmadaki varsayılan kullanılır.</summary>
        public string? LanguageCode { get; init; }

        /// <summary>Para birimi kodu (ör. "EUR").</summary>
        public string? CurrencyCode { get; init; }

        /// <summary>Opsiyonel konum bağlamı (bazı uç noktalar ister).</summary>
        public string? Location { get; init; }
    }
}
