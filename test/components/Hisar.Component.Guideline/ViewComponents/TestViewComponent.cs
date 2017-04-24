using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Threading.Tasks;

namespace Hisar.Component.Guideline.ViewComponents
{
    [ViewComponent(Name = "MyTest")]
    public class TestViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}