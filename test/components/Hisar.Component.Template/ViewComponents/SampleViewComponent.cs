using Hisar.Component.Template.Models;
using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System;
using System.Threading.Tasks;

namespace Hisar.Component.Template.ViewComponents
{
    public class SampleViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View(new GuidelineViewModel { Name = ExecutionComponentId + " Component -> Sample View Component: " + DateTime.Now });
        }
    }
}