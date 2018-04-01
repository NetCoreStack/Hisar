using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetCoreStack.Mvc;

namespace Shared.Library
{
    public class DomainBaseController : Controller
    {
        private IOptions<NetCoreStackMvcOptions> _options;
        protected IOptions<NetCoreStackMvcOptions> Options
        {
            get
            {
                if (_options != null)
                    return _options;

                _options = HttpContext.RequestServices.GetService<IOptions<NetCoreStackMvcOptions>>();
                return _options;
            }
        }

        protected virtual void CreateSuccessNotificationResult(string content)
        {
            var settings = Options.Value;
            ViewBag.Notification = new NotificationResult(settings.AppName, content);
        }

        protected virtual IActionResult CreateSuccessWebResult(string content)
        {
            var settings = Options.Value;
            return Json(new WebResult(settings.AppName, content));
        }
    }
}
