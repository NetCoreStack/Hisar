using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Contracts;
using NetCoreStack.Hisar.Server;
using NetCoreStack.Mvc;
using NetCoreStack.Mvc.Extensions;
using System;
using System.Linq;

namespace Hisar.Component.ContentManagement.Controllers
{
    public class HomeController : HisarControllerServerBase
    {
        private static readonly string _formView = "ContentObjectForm";
        private static readonly string _listView = "ContentObjectList";

        public IActionResult Index()
        {
            return View("ContentObjectForm", new ContentObjectViewModel());
        }

        public IActionResult New()
        {
            return View(_formView, new ContentObjectViewModel());
        }

        public IActionResult List()
        {
            return View(_listView, new ContentObjectViewModel());
        }

        public IActionResult Edit(string id)
        {
            var contentViewModel = UnitOfWork.Repository<ContentObject>()
                .Where(x => x.Id == id)
                .Select(x => new ContentObjectViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Url = x.Url
                }).SingleOrDefault();

            var composer = TryGetComposer<ContentObjectViewModel>();
            composer.Invoke(contentViewModel);

            ViewBag.Id = id;
            return View(_formView, contentViewModel);
        }

        [HttpPost]
        public IActionResult Save([FromBody]ContentObjectViewModel model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                return CreateWebResult(ResultState.Error);
            }

            if (model.IsNew)
            {
                var content = new ContentObject
                {
                    Active = true,
                    ContentObjectType = ContentObjectType.Page,
                    Title = model.Title,
                    Description = model.Description,
                    Url = model.Title.GenerateSlug(),
                    CreatedDate = DateTime.Now,
                    ObjectState = ObjectState.Added
                };

                UnitOfWork.Repository<ContentObject>().SaveAllChanges(content);
                return CreateWebResult(ResultState.Success, redirectUrl: "/home/list");
            }
            else
            {
                var content = UnitOfWork.Repository<ContentObject>().Single(x => x.Id == model.Id);
                if (content == null)
                    throw new ArgumentNullException($"Not Found. Id: {content.Id}");

                content.Title = model.Title;
                content.Description = model.Description;
                content.Url = model.Title.GenerateSlug();

                UnitOfWork.Repository<ContentObject>().SaveAllChanges(content);
                return CreateWebResult(ResultState.Success, redirectUrl: "/home/list");
            }
        }

        public JsonResult Delete(string id)
        {
            var repo = UnitOfWork.Repository<ContentObject>();
            var content = repo.FirstOrDefault(x => x.Id == id);
            if (content != null)
            {
                repo.Delete(content.Id);
                return CreateWebResult(ResultState.Success);
            }

            return CreateWebResult(ResultState.Error);
        }

        public JsonResult GetContentList([FromQuery]CollectionRequest request)
        {
            return Json(UnitOfWork.Repository<ContentObject>()
                .Select(x => new ContentObjectViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url,
                    Active = x.Active
                }).ToCollectionResult(request, TryGetComposer<ContentObjectViewModel>()));
        }
    }
}
