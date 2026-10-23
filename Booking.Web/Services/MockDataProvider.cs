using Booking.Web.Models;

namespace Booking.Web.Services
{
    /// <summary>
    /// Geliştirme ve demo için sahte (mock) veri üretir.
    /// PDF'teki ipucu: API limitlerini korumak için önce mock veriyle
    /// arayüzü kurgulayıp, her şey hazır olunca canlı API'ye bağlanılır.
    /// </summary>
    public static class MockDataProvider
    {
        public static HotelListViewModel GetHotelList(HotelSearchForm form)
        {
            var currency = string.IsNullOrWhiteSpace(form.Currency) ? "EUR" : form.Currency;
            var destination = string.IsNullOrWhiteSpace(form.Destination) ? "Milano" : form.Destination;

            return new HotelListViewModel
            {
                Search = form,
                DestinationLabel = $"{destination}, İtalya",
                IsMockData = true,
                Hotels = new List<HotelCardViewModel>
                {
                    Card(101, "NYX Hotel Milan", "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=800&q=80&auto=format&fit=crop", destination, 512, currency, 8.7, "Çok İyi", 2184, 4),
                    Card(102, "UNAHOTELS Galles Milano", "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=800&q=80&auto=format&fit=crop", destination, 449, currency, 8.5, "Çok İyi", 3412, 4),
                    Card(103, "Rosa Grand Milano", "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=800&q=80&auto=format&fit=crop", destination, 684, currency, 8.9, "Çok İyi", 4026, 4),
                    Card(104, "Hotel Cavour", "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=800&q=80&auto=format&fit=crop", destination, 476, currency, 8.3, "Çok İyi", 1538, 4),
                    Card(105, "NH Collection Milano President", "https://images.unsplash.com/photo-1571896349842-33c89424de2d?w=800&q=80&auto=format&fit=crop", destination, 790, currency, 8.8, "Çok İyi", 2760, 5),
                    Card(106, "Hotel Berna", "https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=800&q=80&auto=format&fit=crop", destination, 428, currency, 8.6, "Çok İyi", 5193, 4),
                }
            };
        }

        public static HotelDetailViewModel GetHotelDetail(long hotelId, HotelSearchForm form)
        {
            var currency = string.IsNullOrWhiteSpace(form.Currency) ? "EUR" : form.Currency;

            return new HotelDetailViewModel
            {
                Search = form,
                HotelId = hotelId,
                Name = "NYX Hotel Milan",
                StarRating = 4,
                Address = "Via Tortona 35",
                City = "Milano",
                Country = "İtalya",
                Latitude = 45.4842,
                Longitude = 9.2040,
                ReviewScore = 8.7,
                ReviewScoreWord = "Çok İyi",
                ReviewCount = 2184,
                Price = 428,
                Currency = currency,
                IsMockData = true,
                Description = "NYX Hotel Milan, Milano Centrale istasyonuna kısa yürüme mesafesinde bulunan modern ve şık bir şehir otelidir. " +
                              "Merkezi konumu sayesinde Duomo, Brera ve Corso Buenos Aires gibi Milano'nun popüler noktalarına kolay ulaşım sağlar. " +
                              "Modern odaları, restoranı ve konforlu ortak alanlarıyla hem iş hem de turistik seyahatler için ideal bir konaklama seçeneğidir.",
                Highlights = new List<string> { "Şehir Merkezi", "Merkezi Konum", "İş Seyahati İçin Uygun", "Çiftler İçin Popüler" },
                Facilities = new List<FacilityViewModel>
                {
                    new() { Name = "Ücretsiz Wi-Fi", Icon = "bi-wifi" },
                    new() { Name = "Kahvaltı", Icon = "bi-cup-hot" },
                    new() { Name = "Klima", Icon = "bi-snow" },
                    new() { Name = "24 Saat Resepsiyon", Icon = "bi-clock" },
                    new() { Name = "Fitness Merkezi", Icon = "bi-heart-pulse" },
                    new() { Name = "Restoran", Icon = "bi-egg-fried" },
                    new() { Name = "Bar", Icon = "bi-cup-straw" },
                    new() { Name = "Otopark", Icon = "bi-p-circle" },
                    new() { Name = "Bagaj Muhafazası", Icon = "bi-suitcase2" },
                },
                PhotoUrls = new List<string>
                {
                    "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb?w=1400&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=900&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1564501049412-61c2a3083791?w=900&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=900&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1552321554-5fefe8c9ef14?w=900&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=900&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1590490360182-c33d57733427?w=900&q=80&auto=format&fit=crop",
                    "https://images.unsplash.com/photo-1534438327276-14e5300c3a48?w=900&q=80&auto=format&fit=crop",
                },
                Rooms = new List<RoomViewModel>
                {
                    new() { RoomId = 201, Name = "Standard Double Room", MaxOccupancy = "2 yetişkin", Price = 428, Currency = currency, PhotoUrl = "https://images.unsplash.com/photo-1590490360182-c33d57733427?w=700&q=80&auto=format&fit=crop" },
                    new() { RoomId = 305, Name = "Superior Room", MaxOccupancy = "2 yetişkin + 1 çocuk", Price = 512, Currency = currency, PhotoUrl = "https://images.unsplash.com/photo-1611892440504-42a792e24d32?w=700&q=80&auto=format&fit=crop" },
                    new() { RoomId = 512, Name = "Junior Suite", MaxOccupancy = "3 kişiye kadar", Price = 684, Currency = currency, PhotoUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=700&q=80&auto=format&fit=crop" },
                }
            };
        }

        private static HotelCardViewModel Card(long id, string name, string photo, string city,
            double price, string currency, double score, string word, int count, int stars) => new()
        {
            HotelId = id,
            Name = name,
            PhotoUrl = photo,
            City = city,
            Country = "İtalya",
            Price = price,
            Currency = currency,
            ReviewScore = score,
            ReviewScoreWord = word,
            ReviewCount = count,
            StarRating = stars
        };
    }
}
