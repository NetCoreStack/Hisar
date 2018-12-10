using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Shared.Library
{
    public class SharedScriptViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.CompletedTask;
            var result = View();

            return result;
        }
    }
}