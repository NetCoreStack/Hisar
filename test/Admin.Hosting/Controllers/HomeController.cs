using Microsoft.AspNetCore.Mvc;
using Shared.Library;

namespace Admin.Hosting.Controllers
{
    public class HomeController : DomainBaseController
    {
        public IActionResult Index()
        {
            CreateSuccessNotificationResult("Welcome...");
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
