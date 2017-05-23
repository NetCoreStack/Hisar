using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Shared.Library
{
    public class FrameRegistrarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            ViewBag.SomeProperty = "some value " + DateTime.Now;
            return View();
        }
    }
}
