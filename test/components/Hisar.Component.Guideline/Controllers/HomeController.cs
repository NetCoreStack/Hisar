using Hisar.CommonLibrary;
using Hisar.Component.Guideline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Contracts;
using NetCoreStack.Hisar;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IUsernamePasswordValidator _validator;
        public HomeController(IUsernamePasswordValidator validator)
        {
            _validator = validator;
            _validator.Validate("foo", "bar");
        }

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
                    AlbumArtUrl = p.AlbumArtUrl,
                    Artist = p.Artist.Name,
                    Tags = p.Tags
                }).ToCollectionResult(request, TryGetComposer<AlbumViewModel>());

            return Json(query);
        }

        [HttpPost(nameof(SaveAlbum))]
        public async Task<AlbumViewModel> SaveAlbum([FromBody]AlbumViewModel model)
        {
            await Task.CompletedTask;

            string id = string.Empty;
            var objectState = ObjectState.Added;
            if (!model.IsNew)
            {
                id = model.Id;
                objectState = ObjectState.Modified;
            }

            var album = new AlbumBson
            {
                AlbumArtUrl = model.AlbumArtUrl,
                Id = id,
                ObjectState = objectState,
                Price = model.Price,
                Title = model.Title,
                UpdatedDate = model.UpdatedDate
            };

            if (!model.IsNew)
                UnitOfWork.Repository<AlbumBson>().Update(album);
            else
                UnitOfWork.Repository<AlbumBson>().Insert(album);
            
            return model;
        }

        public IActionResult Tags(AutoCompleteRequest request)
        {
            var items = new List<IdTextPair>
            {
                new IdTextPair
                {
                    Id = "1",
                    Text = "Brand New"
                },

                new IdTextPair
                {
                    Id = "2",
                    Text = "Millennium"
                },
            };

            return Json(items.ToCollectionResult(request));
        }

        [HttpPut(nameof(UpdateAlbum))]
        public async Task<AlbumViewModel> UpdateAlbum(long id, [FromBody]AlbumViewModel model)
        {
            await Task.CompletedTask;
            return model;
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