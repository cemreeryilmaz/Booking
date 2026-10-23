using System.Diagnostics;
using Booking.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Web.Controllers
{
    public class HomeController : Controller
    {
        // Ana sayfa: arama formunu içeren tema
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
