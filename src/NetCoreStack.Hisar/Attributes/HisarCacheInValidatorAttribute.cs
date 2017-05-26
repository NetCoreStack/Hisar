using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCoreStack.Hisar
{
    public class HisarCacheInValidatorAttribute : ActionFilterAttribute
    {
        public string[] Keys { get; set; }

        public HisarCacheInValidatorAttribute(params string[] keys)
        {
            Keys = keys ?? throw new ArgumentNullException($"Cache keys could not be null, parameter: {nameof(keys)}");
        }

        public ICommonCacheProvider GetCacheProvider(ActionContext context)
        {
            return context.HttpContext.RequestServices.GetService<ICommonCacheProvider>();
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Exception == null)
            {
                var cacheProvider = GetCacheProvider(context);
                cacheProvider.Remove(Keys);
            }
        }
    }
}