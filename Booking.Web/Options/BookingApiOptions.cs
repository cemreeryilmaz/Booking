namespace Booking.Web.Options
{
    /// <summary>
    /// RapidAPI üzerindeki Booking.com API'si için yapılandırma ayarları.
    /// Değerler appsettings.json içindeki "BookingApi" bölümünden okunur.
    /// </summary>
    public class BookingApiOptions
    {
        public const string SectionName = "BookingApi";

        /// <summary>API'nin temel adresi (örn: https://booking-com15.p.rapidapi.com).</summary>
        public string BaseUrl { get; set; } = "https://booking-com15.p.rapidapi.com";

        /// <summary>RapidAPI anahtarı (x-rapidapi-key). Asla public repoya yüklenmez.</summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>RapidAPI host başlığı (x-rapidapi-host).</summary>
        public string ApiHost { get; set; } = "booking-com15.p.rapidapi.com";

        /// <summary>Dil kodu (örn: en-us). Booking API bu değeri bekler.</summary>
        public string LanguageCode { get; set; } = "en-us";

        /// <summary>
        /// true ise gerçek API yerine mock (sahte JSON) veri kullanılır.
        /// Geliştirme sırasında istek limitini korumak için faydalıdır.
        /// </summary>
        public bool UseMockData { get; set; } = false;

        /// <summary>
        /// true ise canlı API hata verdiğinde ya da boş döndüğünde
        /// arayüzün bozulmaması için otomatik olarak mock veriye düşülür.
        /// </summary>
        public bool FallbackToMockOnError { get; set; } = true;

        /// <summary>
        /// true ise otel detayında açıklama boş geldiğinde
        /// <c>getDescriptionAndInfo</c> uç noktasıyla ek bir istek atılarak metin doldurulur.
        /// İstek limitini korumak isteyenler false yapabilir.
        /// </summary>
        public bool EnrichDetailWithDescription { get; set; } = true;
    }
}
