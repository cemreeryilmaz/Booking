using Booking.Web.Models;
using Booking.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IBookingApiService _bookingApiService;

        public HotelController(IBookingApiService bookingApiService)
        {
            _bookingApiService = bookingApiService;
        }

        // 2. ADIM: Otel listeleme
        // GET /Hotel/HotelList?Destination=Milano&CheckIn=...&CheckOut=...&Adults=2&...
        [HttpGet]
        public async Task<IActionResult> HotelList(HotelSearchForm search)
        {
            // Hiç arama yapılmadan gelindiyse kullanıcıyı ana sayfaya yönlendir.
            if (string.IsNullOrWhiteSpace(search.Destination))
                return RedirectToAction("Index", "Home");

            var model = await _bookingApiService.SearchHotelsAsync(search);
            return View(model);
        }

        // 3. ADIM: Otel detay
        // GET /Hotel/HotelDetail?hotelId=123&Destination=Milano&CheckIn=...
        [HttpGet]
        public async Task<IActionResult> HotelDetail(long hotelId, HotelSearchForm search)
        {
            if (hotelId <= 0)
                return RedirectToAction("Index", "Home");

            var model = await _bookingApiService.GetHotelDetailAsync(hotelId, search);
            return View(model);
        }
    }
}
