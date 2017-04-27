using Microsoft.AspNetCore.Mvc;
using System;

namespace Hisar.Component.Template.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.StrProperty = DateTime.Now.ToString();
            return View();
        }

        public IActionResult Registration()
        {
            return View(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}