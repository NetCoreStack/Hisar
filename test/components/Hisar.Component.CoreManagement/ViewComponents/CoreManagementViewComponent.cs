using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hisar.Component.CoreManagement.ViewComponents
{
    public class CoreManagementViewComponent : HisarViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}
