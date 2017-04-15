using NetCoreStack.Hisar.Server;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Hisar.Component.Guideline.ViewComponents
{
    public class Guideline2ViewComponent : HisarServerViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}