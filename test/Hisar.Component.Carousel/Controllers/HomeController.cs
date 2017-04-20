using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;

namespace Hisar.Component.Carousel.Controllers
{
    [HisarRoute(nameof(Carousel))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}