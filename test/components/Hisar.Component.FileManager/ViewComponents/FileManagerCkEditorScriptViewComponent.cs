using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;

namespace Hisar.Component.FileManager.ViewComponents
{
    public class FileManagerCkEditorScriptViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
