using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Threading.Tasks;

namespace Hisar.Component.FileManager.ViewComponents
{
    public class FileManagerViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
