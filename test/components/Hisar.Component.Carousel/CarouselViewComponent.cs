using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Threading.Tasks;

namespace Hisar.Component.Carousel
{
    public class CarouselViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
