using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System;
using System.Threading.Tasks;

namespace Hisar.Component.Carousel.ViewComponents
{
    public class SampleViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View(new CarouselViewModel { Title = ExecutionComponentId + " Component -> Sample View Component: " + DateTime.Now });
        }
    }
}