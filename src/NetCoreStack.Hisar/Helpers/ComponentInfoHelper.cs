using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace NetCoreStack.Hisar
{
    public class ComponentInfoHelper
    {
        public static RunningComponentHelper GetComponentHelper(ActionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<RunningComponentHelper>(context.HttpContext.RequestServices);
        }
    }
}
