using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Threading.Tasks;

namespace Shared.Library
{
    public class SharedScriptViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
