using Hisar.Component.Guideline.Models;
using NetCoreStack.Hisar;
using Microsoft.AspNetCore.Mvc;

namespace Hisar.Component.Guideline.Controllers
{
    [HisarRoute(nameof(Guideline))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(GuidelineViewModel model)
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrationJson([FromBody]GuidelineViewModel model)
        {
            return Json(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}