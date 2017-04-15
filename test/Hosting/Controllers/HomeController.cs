using NetCoreStack.Hisar.Server;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Hosting.Controllers
{
    public class HomeController : HisarControllerServerBase
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}