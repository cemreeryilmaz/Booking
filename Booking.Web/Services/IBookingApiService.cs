using Booking.Web.Models;

namespace Booking.Web.Services
{
    /// <summary>
    /// Booking.com API akışını soyutlar. Controller'lar sadece bu arayüzü bilir;
    /// canlı API mı yoksa mock veri mi kullanıldığını bilmeleri gerekmez.
    /// </summary>
    public interface IBookingApiService
    {
        /// <summary>
        /// 1 ve 2. adımları çalıştırır: şehir adından dest_id bulur, ardından
        /// otelleri listeler.
        /// </summary>
        Task<HotelListViewModel> SearchHotelsAsync(HotelSearchForm form);

        /// <summary>3. adım: seçilen otelin detaylarını getirir.</summary>
        Task<HotelDetailViewModel> GetHotelDetailAsync(long hotelId, HotelSearchForm form);
    }
}
