using System.Globalization;
using System.Text.Json;
using Booking.Web.Models.Api;

namespace Booking.Web.Services.Api
{
    /// <summary>
    /// <see cref="IHotelsApi"/> uygulaması. Her metot ilgili <c>/api/v1/hotels/*</c> uç noktasının
    /// URL + sorgu parametrelerini kurar ve <see cref="BookingApiClient"/> üzerinden çağırır.
    /// </summary>
    public class HotelsApi : IHotelsApi
    {
        private const string Prefix = "/api/v1/hotels";
        private readonly BookingApiClient _client;

        public HotelsApi(BookingApiClient client) => _client = client;

        private string Lang(string? languageCode) =>
            string.IsNullOrWhiteSpace(languageCode) ? _client.DefaultLanguageCode : languageCode!;

        private static string Num(double value) => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>Konaklama (tarih/misafir/birim) parametrelerini sorgu sözlüğüne çevirir.</summary>
        private Dictionary<string, string?> StayToQuery(StayParams stay) => new()
        {
            ["arrival_date"] = stay.ArrivalDate,
            ["departure_date"] = stay.DepartureDate,
            ["adults"] = stay.Adults.ToString(),
            ["children_age"] = stay.ChildrenAge,
            ["room_qty"] = stay.RoomQty.ToString(),
            ["page_number"] = stay.PageNumber.ToString(),
            ["units"] = stay.Units,
            ["temperature_unit"] = stay.TemperatureUnit,
            ["languagecode"] = Lang(stay.LanguageCode),
            ["currency_code"] = stay.CurrencyCode,
            ["location"] = stay.Location
        };

        public Task<JsonElement> SearchDestinationAsync(string query, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/searchDestination", new Dictionary<string, string?>
            {
                ["query"] = query,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetFilterAsync(string destId, string searchType, StayParams stay, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["dest_id"] = destId;
            q["search_type"] = searchType;
            return _client.GetRawAsync($"{Prefix}/getFilter", q, ct);
        }

        public Task<JsonElement> GetSortByAsync(string destId, string searchType, StayParams stay, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["dest_id"] = destId;
            q["search_type"] = searchType;
            return _client.GetRawAsync($"{Prefix}/getSortBy", q, ct);
        }

        public Task<JsonElement> SearchHotelsAsync(string destId, string searchType, StayParams stay,
            string? categoriesFilterIds = null, string? sortBy = null, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["dest_id"] = destId;
            q["search_type"] = searchType;
            q["categories_filter_ids"] = categoriesFilterIds;
            q["sort_by"] = sortBy;
            return _client.GetRawAsync($"{Prefix}/searchHotels", q, ct);
        }

        public Task<JsonElement> SearchHotelsByCoordinatesAsync(double latitude, double longitude, StayParams stay, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["latitude"] = Num(latitude);
            q["longitude"] = Num(longitude);
            return _client.GetRawAsync($"{Prefix}/searchHotelsByCoordinates", q, ct);
        }

        public Task<JsonElement> GetHotelDetailsAsync(long hotelId, StayParams stay, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["hotel_id"] = hotelId.ToString();
            return _client.GetRawAsync($"{Prefix}/getHotelDetails", q, ct);
        }

        public Task<JsonElement> GetAvailabilityAsync(long hotelId, string? minDate = null, string? maxDate = null,
            string? currencyCode = null, string? location = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getAvailability", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["min_date"] = minDate,
                ["max_date"] = maxDate,
                ["currency_code"] = currencyCode,
                ["location"] = location
            }, ct);

        public Task<JsonElement> GetDescriptionAndInfoAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getDescriptionAndInfo", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetRoomListAsync(long hotelId, StayParams stay, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["hotel_id"] = hotelId.ToString();
            return _client.GetRawAsync($"{Prefix}/getRoomList", q, ct);
        }

        public Task<JsonElement> GetRoomListWithAvailabilityAsync(long hotelId, StayParams stay, CancellationToken ct = default)
        {
            var q = StayToQuery(stay);
            q["hotel_id"] = hotelId.ToString();
            return _client.GetRawAsync($"{Prefix}/getRoomListWithAvailability", q, ct);
        }

        public Task<JsonElement> GetPaymentFeaturesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getPaymentFeatures", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelPoliciesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelPolicies", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetPropertyChildrenPoliciesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/propertyChildrenPolicies", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelReviewsFilterMetadataAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelReviewsFilterMetadata", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelReviewScoresAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelReviewScores", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelReviewsSortOptionAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelReviewsSortOption", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelReviewsAsync(long hotelId, int pageNumber = 1, string? sortOption = null, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelReviews", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["page_number"] = pageNumber.ToString(),
                ["sort_option_id"] = sortOption,
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetQuestionAndAnswerAsync(long hotelId, int pageNumber = 1, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getQuestionAndAnswer", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["page_number"] = pageNumber.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetNearbyCitiesAsync(double latitude, double longitude, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getNearbyCities", new Dictionary<string, string?>
            {
                ["latitude"] = Num(latitude),
                ["longitude"] = Num(longitude),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetPropertySurroundingsAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getPropertySurroundings", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetPopularAttractionNearByAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getPopularAttractionNearBy", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelPhotosAsync(long hotelId, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelPhotos", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString()
            }, ct);

        public Task<JsonElement> GetHotelFacilitiesAsync(long hotelId, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelFacilities", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["languagecode"] = Lang(languageCode)
            }, ct);

        public Task<JsonElement> GetHotelCheckInOutTimeAsync(long hotelId, string arrivalDate, string departureDate, string? languageCode = null, CancellationToken ct = default)
            => _client.GetRawAsync($"{Prefix}/getHotelCheckInOutTime", new Dictionary<string, string?>
            {
                ["hotel_id"] = hotelId.ToString(),
                ["arrival_date"] = arrivalDate,
                ["departure_date"] = departureDate,
                ["languagecode"] = Lang(languageCode)
            }, ct);
    }
}
