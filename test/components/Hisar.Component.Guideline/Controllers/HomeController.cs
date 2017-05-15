using Hisar.CommonLibrary;
using Hisar.Component.Guideline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Contracts;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hisar.Component.Guideline.Controllers
{
    public class Cached
    {
        public static List<IdTextPair> Artists { get; set; }
        public static List<IdTextPair> Genres { get; set; }

        static Cached()
        {
            Artists = new List<IdTextPair>();
            Genres = new List<IdTextPair>();
        }
    }

    public class HomeController : HisarControllerServerBase
    {
        public IActionResult Index()
        {
            var externalLibrary = new ExternalLibrary();
            ViewBag.ExternalLibrary = externalLibrary.Name;

            if (!Cached.Artists.Any())
            {
                Cached.Artists = UnitOfWork.Repository<ArtistBson>().Select(a => new IdTextPair { Id = a.Id, Text = a.Name }).ToList();
                Cached.Genres = UnitOfWork.Repository<GenreBson>().Select(a => new IdTextPair { Id = a.Id, Text = a.Name }).ToList();
            }

            ViewBag.Artists = Cached.Artists;
            ViewBag.Genres = Cached.Genres;

            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Albums()
        {
            return View();
        }

        public IActionResult GetAlbums(CollectionRequest request)
        {
            var query = UnitOfWork.Repository<AlbumBson>()
                .Select(p => new AlbumViewModel {
                    Id = p.Id,
                    Genre = p.Genre.Name,
                    Price = p.Price,
                    Title = p.Title,
                    Artist = p.Artist.Name
                }).ToCollectionResult(request);

            return Json(query);
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