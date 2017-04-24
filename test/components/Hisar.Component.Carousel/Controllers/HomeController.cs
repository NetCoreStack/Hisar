using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Carousel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}