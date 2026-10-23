# StayFinder — Booking.com API Entegrasyonu (Vaka Çalışması)

Statik olarak verilen HTML/CSS temalarını, **RapidAPI** üzerindeki **Booking.com (booking-com15)**
API'sini kullanarak dinamik ve çalışan bir otel arama motoruna dönüştüren
**ASP.NET Core MVC** uygulaması.

Uygulama üç temel işlevi yerine getirir:

1. **Otel Arama & Listeleme** — Şehir, tarih, misafir ve para birimi bilgileriyle otel arama.
2. **Otel Listesi** — Sonuçların kart halinde (isim, fotoğraf, fiyat, puan) gösterimi.
3. **Otel Detayı** — Seçilen otele ait adres, imkanlar, odalar ve fotoğraf galerisi.

---

## 🧩 Teknolojiler

- **.NET 8** / ASP.NET Core MVC
- `HttpClient` (typed client) + `System.Text.Json`
- Bootstrap 5 + Bootstrap Icons (CDN) — StayFinder teması
- RapidAPI · Booking.com API (`booking-com15.p.rapidapi.com`)

---

## 🔌 Ana Akışta Kullanılan Uç Noktalar (3 adım)

| Adım | Endpoint | İşlev |
|------|----------|-------|
| 1. Lokasyon | `GET /api/v1/hotels/searchDestination?query={şehir}` | Şehir adından `dest_id` elde edilir. |
| 2. Otel Arama | `GET /api/v1/hotels/searchHotels?dest_id=...&arrival_date=...&departure_date=...&adults=...&room_qty=...&currency_code=...` | `dest_id` ve form parametreleriyle otel listesi çekilir. |
| 3. Otel Detay | `GET /api/v1/hotels/getHotelDetails?hotel_id=...&arrival_date=...&...` | Seçilen otelin detayları çekilir. |
| 3b. Açıklama | `GET /api/v1/hotels/getDescriptionAndInfo?hotel_id=...` | Detay sayfasındaki "Otel Hakkında" metni bu uç noktayla zenginleştirilir. |

Akış: **Form → (1) Lokasyon bul → (2) Otelleri listele → (3) Otel detayını göster.**

---

## 🌐 Tüm Endpoint'ler (Servis Katmanı)

Ana akıştaki 3 uç noktanın yanında, **booking-com15 API'sinin tüm kategorileri** (toplam 55+ uç nokta)
tipli bir servis katmanı olarak projeye dahil edilmiştir. Her kategori kendi arayüzü/servisiyle
`Services/Api` altında yer alır ve ortak `BookingApiClient` üzerinden çağrılır:

| Kategori | Arayüz | Örnek uç noktalar |
|----------|--------|-------------------|
| Hotels | `IHotelsApi` | searchDestination, searchHotels, searchHotelsByCoordinates, getHotelDetails, getDescriptionAndInfo, getRoomList(WithAvailability), getAvailability, getHotelPhotos, getHotelFacilities, getHotelReviews(+Scores/+SortOption/+FilterMetadata), getQuestionAndAnswer, getHotelPolicies, propertyChildrenPolicies, getPaymentFeatures, getFilter, getSortBy, getPropertySurroundings, getPopularAttractionNearBy, getNearbyCities, getHotelCheckInOutTime |
| Flights | `IFlightsApi` | searchDestination, searchFlights, searchFlightsMultiStops, getFlightDetails, getMinPrice(+MultiStops), getSeatMap |
| Cars | `ICarsApi` | v1: searchDestination, searchCarRentals, getPackages, vehicleDetails, bookingSummary, vehicleSupplier(Details/Ratings/Review) · v2: searchDestination, searchCarRentals, vehicleDetails |
| Taxi | `ITaxiApi` | searchLocation, searchTaxi |
| Attraction | `IAttractionsApi` | searchLocation, searchAttractions, getAttractionDetails, getAvailabilityCalendar, getAvailability |
| Meta | `IMetaApi` | getLanguages, getCurrency, getExchangeRates, locationToLatLong, getLocations, test |

### 🧭 API Gezgini

`/ApiExplorer` adresinde, projeye entegre edilen tüm uç noktaları kategori kategori listeleyen
ve parametresiz/metin aramalı güvenli bir alt kümeyi **canlı çağırıp ham JSON yanıtını gösteren**
bir geliştirici sayfası bulunur (üst menüdeki "API Gezgini").

---

## 🚀 Çalıştırma

### Gereksinimler
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (veya üzeri)
- Geçerli bir RapidAPI anahtarı (Booking.com API aboneliği)

### Adımlar

```bash
# 1) API anahtarını yapılandır
#    Booking.Web/appsettings.Example.json dosyasını kopyalayıp
#    Booking.Web/appsettings.json olarak kaydedin ve ApiKey alanını doldurun.

# 2) Çalıştır
cd Booking.Web
dotnet run
```

Tarayıcıda açılır: `http://localhost:5214` (veya `https://localhost:7245`).

---

## 🔑 API Anahtarı Yapılandırması

Anahtar **koda gömülmez**; `appsettings.json` içindeki `BookingApi` bölümünde tutulur
ve `.gitignore` sayesinde **GitHub'a yüklenmez.**

```jsonc
// Booking.Web/appsettings.json  (bu dosya .gitignore'dadır)
{
  "BookingApi": {
    "BaseUrl": "https://booking-com15.p.rapidapi.com",
    "ApiKey": "RAPIDAPI_ANAHTARINIZ",
    "ApiHost": "booking-com15.p.rapidapi.com",
    "LanguageCode": "en-us",
    "UseMockData": false,              // true → gerçek API yerine sahte veri kullan
    "FallbackToMockOnError": true,     // API hata verirse arayüz bozulmasın diye mock'a düş
    "EnrichDetailWithDescription": true // detay sayfasında açıklamayı getDescriptionAndInfo ile doldur
  }
}
```

Depoda örnek şablon olarak `appsettings.Example.json` bulunur (placeholder anahtar ile).

---

## 🧪 Mock (Sahte) Veri Modu

PDF'teki ipucuna uygun olarak, geliştirme sırasında istek limitini korumak için
`UseMockData: true` yapılarak uygulama **hiç API isteği atmadan** örnek veriyle çalıştırılabilir.
Ayrıca `FallbackToMockOnError: true` iken canlı API'ye ulaşılamazsa arayüz otomatik olarak
demo veriyle doldurulur (kullanıcıya bir bilgi rozeti gösterilir).

---

## 🗂️ Proje Yapısı

```
Booking.Web/
├─ Controllers/
│  ├─ HomeController.cs        # Ana sayfa (arama formu)
│  ├─ HotelController.cs       # HotelList (2. adım) + HotelDetail (3. adım)
│  └─ ApiExplorerController.cs # Tüm uç noktaların kataloğu + canlı test konsolu
├─ Services/
│  ├─ IBookingApiService.cs    # Otel akışı soyutlaması
│  ├─ BookingApiService.cs     # Canlı RapidAPI çağrıları + açıklama zenginleştirme + hata yönetimi
│  ├─ MockDataProvider.cs      # Sahte (demo) veri
│  └─ Api/                     # Tüm endpoint'leri kapsayan tipli servis katmanı
│     ├─ BookingApiClient.cs   # Ortak düşük seviyeli HTTP istemcisi (header/URL/JSON)
│     ├─ IHotelsApi / HotelsApi.cs
│     ├─ IFlightsApi / FlightsApi.cs
│     ├─ ICarsApi / CarsApi.cs
│     ├─ ITaxiApi / TaxiApi.cs
│     ├─ IAttractionsApi / AttractionsApi.cs
│     └─ IMetaApi / MetaApi.cs
├─ DTOs/Booking/               # API JSON yanıtlarını karşılayan modeller
│  ├─ LocationSearchDtos.cs
│  ├─ HotelSearchDtos.cs
│  └─ HotelDetailsDtos.cs
├─ Models/                     # Görünümlerin kullandığı temiz ViewModel'ler
│  ├─ HotelSearchForm.cs
│  ├─ HotelCardViewModel.cs
│  ├─ HotelListViewModel.cs
│  └─ HotelDetailViewModel.cs
├─ Options/BookingApiOptions.cs
├─ Views/
│  ├─ Home/Index.cshtml        # Tema: Ana sayfa + arama formu
│  ├─ Hotel/HotelList.cshtml   # Tema: Otel listesi
│  ├─ Hotel/HotelDetail.cshtml # Tema: Otel detayı (+ demo rezervasyon modalı)
│  ├─ ApiExplorer/Index.cshtml # Tüm uç noktaların listesi + canlı test konsolu
│  └─ Shared/_Layout.cshtml, _HotelCard.cshtml
├─ wwwroot/css/site.css        # Ortak StayFinder stilleri (+ toast bildirimi)
├─ wwwroot/js/site.js          # Sayaçlar, favori, slider, loading, paylaş, rezervasyon (demo)
├─ appsettings.json            # (gitignore) gerçek API anahtarı
└─ appsettings.Example.json    # Örnek şablon
```

---

## ✅ Değerlendirme Kriterlerine Karşılık

- **API Entegrasyonu:** Form verileri doğru endpoint'lere parametre olarak iletilir; dönen JSON `System.Text.Json` ile DTO'lara ayrıştırılır.
- **DOM / Dinamik Render:** API verileri Razor görünümlerinde (başlık, görsel, fiyat, puan vb.) ilgili alanlara basılır; liste `_HotelCard` partial'ı ile modüler render edilir.
- **Hata Yönetimi:** Şehir bulunamazsa / sonuç dönmezse / ağ hatası olursa kullanıcıya uygun uyarı gösterilir; arama sırasında "loading" katmanı çıkar.
- **Kod Kalitesi:** Servis katmanı (`IBookingApiService`), DTO ↔ ViewModel ayrımı, anlamlı isimlendirme ve yorumlar.

---

## 📝 Teslim Notları

- Kaynak kodları bir GitHub reposuna yükleyin.
- **`appsettings.json` (API anahtarınız) repoya yüklenmez** — `.gitignore` ile dışlanmıştır.
- Çalıştırma talimatları bu README'dedir.
