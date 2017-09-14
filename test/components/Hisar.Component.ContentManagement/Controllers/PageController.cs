using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar.Server;

namespace Hisar.Component.ContentManagement.Controllers
{
    [Route("page/{id?}")]
    [ResponseCache(Duration = 20)]
    public class PageController : HisarControllerServerBase
    {
        public IActionResult Index(string id)
        {
            var model = GetOrCreateCacheItem<ContentObjectViewModel>(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(ResolveViewName("Index"), model);
        }
    }
}
