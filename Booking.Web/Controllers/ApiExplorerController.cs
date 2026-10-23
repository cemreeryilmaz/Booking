using System.Text.Json;
using Booking.Web.Models;
using Booking.Web.Services.Api;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.Controllers
{
    /// <summary>
    /// Geliştirici/değerlendirici amaçlı "API Gezgini" sayfası.
    /// booking-com15 API'sindeki tüm uç noktaları kategori kategori listeler ve
    /// parametre gerektirmeyen (ya da yalnızca metin arayan) güvenli bir alt kümeyi
    /// canlı olarak çağırıp ham JSON yanıtı gösterir.
    /// </summary>
    public class ApiExplorerController : Controller
    {
        private readonly IMetaApi _meta;
        private readonly IHotelsApi _hotels;
        private readonly IFlightsApi _flights;
        private readonly ICarsApi _cars;
        private readonly ITaxiApi _taxi;
        private readonly IAttractionsApi _attractions;

        private static readonly JsonSerializerOptions Pretty = new() { WriteIndented = true };

        public ApiExplorerController(IMetaApi meta, IHotelsApi hotels, IFlightsApi flights,
            ICarsApi cars, ITaxiApi taxi, IAttractionsApi attractions)
        {
            _meta = meta;
            _hotels = hotels;
            _flights = flights;
            _cars = cars;
            _taxi = taxi;
            _attractions = attractions;
        }

        // Tüm endpoint kataloğu (kategori -> uç noktalar). Canlı denenebilenler IsLive = true.
        public static readonly IReadOnlyList<ApiEndpointGroup> Catalog = BuildCatalog();

        [HttpGet]
        public IActionResult Index() => View(Catalog);

        /// <summary>
        /// Güvenli (parametre gerektirmeyen ya da sadece "q" metni arayan) uç noktaları canlı çağırır.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Call(string group, string name, string? q, CancellationToken ct)
        {
            try
            {
                var json = await InvokeAsync(group, name, q, ct);
                var text = JsonSerializer.Serialize(json, Pretty);
                return Content(text, "application/json; charset=utf-8");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 502;
                return Content($"{{\n  \"hata\": \"{ex.Message.Replace("\"", "'")}\"\n}}", "application/json; charset=utf-8");
            }
        }

        private async Task<JsonElement> InvokeAsync(string group, string name, string? q, CancellationToken ct)
        {
            var query = string.IsNullOrWhiteSpace(q) ? "Istanbul" : q;

            return (group, name) switch
            {
                ("meta", "test") => await _meta.TestAsync(ct),
                ("meta", "getLanguages") => await _meta.GetLanguagesAsync(ct),
                ("meta", "getCurrency") => await _meta.GetCurrencyAsync(ct),
                ("meta", "getExchangeRates") => await _meta.GetExchangeRatesAsync(string.IsNullOrWhiteSpace(q) ? "EUR" : q, ct),
                ("meta", "locationToLatLong") => await _meta.LocationToLatLongAsync(query, ct),
                ("meta", "getLocations") => await _meta.GetLocationsAsync(query, ct),

                ("hotels", "searchDestination") => await _hotels.SearchDestinationAsync(query, ct: ct),
                ("flights", "searchDestination") => await _flights.SearchDestinationAsync(query, ct: ct),
                ("cars", "searchDestination") => await _cars.SearchDestinationAsync(query, ct),
                ("taxi", "searchLocation") => await _taxi.SearchLocationAsync(query, ct: ct),
                ("attraction", "searchLocation") => await _attractions.SearchLocationAsync(query, ct: ct),

                _ => throw new InvalidOperationException(
                    "Bu uç nokta canlı gezginde desteklenmiyor (ID/zincirli parametre gerektiriyor). " +
                    "Servis katmanından programatik olarak çağrılabilir.")
            };
        }

        private static IReadOnlyList<ApiEndpointGroup> BuildCatalog() => new List<ApiEndpointGroup>
        {
            new("Meta", new()
            {
                new("meta", "getLanguages", "/api/v1/meta/getLanguages", "Desteklenen diller", true),
                new("meta", "getCurrency", "/api/v1/meta/getCurrency", "Para birimleri", true),
                new("meta", "getExchangeRates", "/api/v1/meta/getExchangeRates", "Döviz kurları (q = taban para birimi)", true),
                new("meta", "locationToLatLong", "/api/v1/meta/locationToLatLong", "Adres → enlem/boylam (q = adres)", true),
                new("meta", "getLocations", "/api/v1/meta/getLocations", "Genel konum araması (q = ad)", true),
                new("meta", "test", "/api/v1/test", "Sunucu ayakta mı?", true),
            }),
            new("Hotels", new()
            {
                new("hotels", "searchDestination", "/api/v1/hotels/searchDestination", "Lokasyon arama (q = şehir)", true),
                new("hotels", "getFilter", "/api/v1/hotels/getFilter", "Filtre seçenekleri", false),
                new("hotels", "getSortBy", "/api/v1/hotels/getSortBy", "Sıralama seçenekleri", false),
                new("hotels", "searchHotels", "/api/v1/hotels/searchHotels", "Otel listeleme (uygulamada kullanılıyor)", false),
                new("hotels", "searchHotelsByCoordinates", "/api/v1/hotels/searchHotelsByCoordinates", "Koordinata göre otel", false),
                new("hotels", "getHotelDetails", "/api/v1/hotels/getHotelDetails", "Otel detayı (uygulamada kullanılıyor)", false),
                new("hotels", "getAvailability", "/api/v1/hotels/getAvailability", "İleri tarih müsaitliği", false),
                new("hotels", "getDescriptionAndInfo", "/api/v1/hotels/getDescriptionAndInfo", "Açıklama (detayda kullanılıyor)", false),
                new("hotels", "getRoomList", "/api/v1/hotels/getRoomList", "Oda listesi", false),
                new("hotels", "getRoomListWithAvailability", "/api/v1/hotels/getRoomListWithAvailability", "Müsaitlikli oda listesi", false),
                new("hotels", "getPaymentFeatures", "/api/v1/hotels/getPaymentFeatures", "Ödeme özellikleri", false),
                new("hotels", "getHotelPolicies", "/api/v1/hotels/getHotelPolicies", "Otel politikaları", false),
                new("hotels", "propertyChildrenPolicies", "/api/v1/hotels/propertyChildrenPolicies", "Çocuk politikaları", false),
                new("hotels", "getHotelReviewsFilterMetadata", "/api/v1/hotels/getHotelReviewsFilterMetadata", "Yorum filtre meta verisi", false),
                new("hotels", "getHotelReviewScores", "/api/v1/hotels/getHotelReviewScores", "Değerlendirme puanları", false),
                new("hotels", "getHotelReviewsSortOption", "/api/v1/hotels/getHotelReviewsSortOption", "Yorum sıralama seçenekleri", false),
                new("hotels", "getHotelReviews", "/api/v1/hotels/getHotelReviews", "Otel yorumları", false),
                new("hotels", "getQuestionAndAnswer", "/api/v1/hotels/getQuestionAndAnswer", "Soru-cevap", false),
                new("hotels", "getNearbyCities", "/api/v1/hotels/getNearbyCities", "Yakın şehirler (deprecated)", false),
                new("hotels", "getPropertySurroundings", "/api/v1/hotels/getPropertySurroundings", "Çevredeki ilgi noktaları", false),
                new("hotels", "getPopularAttractionNearBy", "/api/v1/hotels/getPopularAttractionNearBy", "Yakın popüler yerler", false),
                new("hotels", "getHotelPhotos", "/api/v1/hotels/getHotelPhotos", "Otel fotoğrafları", false),
                new("hotels", "getHotelFacilities", "/api/v1/hotels/getHotelFacilities", "Otel imkanları", false),
                new("hotels", "getHotelCheckInOutTime", "/api/v1/hotels/getHotelCheckInOutTime", "Giriş/çıkış saatleri", false),
            }),
            new("Flights", new()
            {
                new("flights", "searchDestination", "/api/v1/flights/searchDestination", "Havalimanı arama (q)", true),
                new("flights", "searchFlights", "/api/v1/flights/searchFlights", "Uçuş arama", false),
                new("flights", "searchFlightsMultiStops", "/api/v1/flights/searchFlightsMultiStops", "Çok duraklı uçuş", false),
                new("flights", "getFlightDetails", "/api/v1/flights/getFlightDetails", "Uçuş detayı", false),
                new("flights", "getMinPrice", "/api/v1/flights/getMinPrice", "En düşük fiyat", false),
                new("flights", "getMinPriceMultiStops", "/api/v1/flights/getMinPriceMultiStops", "Çok duraklı en düşük fiyat", false),
                new("flights", "getSeatMap", "/api/v1/flights/getSeatMap", "Koltuk haritası", false),
            }),
            new("Cars", new()
            {
                new("cars", "searchDestination", "/api/v1/cars/searchDestination", "Araç lokasyon arama (q)", true),
                new("cars", "searchCarRentals", "/api/v1/cars/searchCarRentals", "Araç kiralama arama", false),
                new("cars", "getPackages", "/api/v1/cars/getPackages", "Paketler", false),
                new("cars", "vehicleDetails", "/api/v1/cars/vehicleDetails", "Araç detayı", false),
                new("cars", "bookingSummary", "/api/v1/cars/bookingSummary", "Rezervasyon özeti", false),
                new("cars", "vehicleSupplierDetails", "/api/v1/cars/vehicleSupplierDetails", "Tedarikçi detayı", false),
                new("cars", "vehicleSupplierRatings", "/api/v1/cars/vehicleSupplierRatings", "Tedarikçi puanları", false),
                new("cars", "vehicleSupplierReview", "/api/v1/cars/vehicleSupplierReview", "Tedarikçi yorumları", false),
                new("cars", "searchDestinationV2", "/api/v2/cars/searchDestination", "Araç lokasyon arama (v2)", false),
                new("cars", "searchCarRentalsV2", "/api/v2/cars/searchCarRentals", "Araç kiralama arama (v2)", false),
                new("cars", "vehicleDetailsV2", "/api/v2/cars/vehicleDetails", "Araç detayı (v2)", false),
            }),
            new("Taxi", new()
            {
                new("taxi", "searchLocation", "/api/v1/taxi/searchLocation", "Taksi konum arama (q)", true),
                new("taxi", "searchTaxi", "/api/v1/taxi/searchTaxi", "Taksi/transfer arama", false),
            }),
            new("Attraction", new()
            {
                new("attraction", "searchLocation", "/api/v1/attraction/searchLocation", "Gezilecek yer lokasyonu (q)", true),
                new("attraction", "searchAttractions", "/api/v1/attraction/searchAttractions", "Gezilecek yer listeleme", false),
                new("attraction", "getAttractionDetails", "/api/v1/attraction/getAttractionDetails", "Gezilecek yer detayı", false),
                new("attraction", "getAvailabilityCalendar", "/api/v1/attraction/getAvailabilityCalendar", "Müsaitlik takvimi", false),
                new("attraction", "getAvailability", "/api/v1/attraction/getAvailability", "Tarihe göre müsaitlik", false),
            }),
        };
    }
}
