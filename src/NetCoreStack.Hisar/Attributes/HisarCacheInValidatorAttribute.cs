using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Mvc.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class HisarCacheInValidatorAttribute : ActionFilterAttribute
    {
        public string[] Keys { get; set; }

        public HisarCacheInValidatorAttribute(params string[] keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException($"Cache keys could not be null, parameter: {nameof(keys)}");
            }

            Keys = keys;
        }

        public IEnumerable<IHisarCacheValueProvider> GetCacheValueProviders(ActionContext context)
        {
            return context.HttpContext.RequestServices.GetServices<IHisarCacheValueProvider>();
        }

        public ICommonCacheProvider GetCacheProvider(ActionContext context)
        {
            return context.HttpContext.RequestServices.GetService<ICommonCacheProvider>();
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Exception == null)
            {
                var cacheValueProviders = GetCacheValueProviders(context).ToList();
                foreach (var valueProvider in cacheValueProviders)
                {
                    foreach (var key in Keys)
                    {
                        DateTimeOffset? absoluteExpiration = null;

                        var value = valueProvider.GetValueSetter(key, ref absoluteExpiration);
                        if (value != null)
                        {
                            var cacheProvider = GetCacheProvider(context);
                            if (cacheProvider != null)
                            {
                                cacheProvider.SetObject(key, value, new CacheProviderOptions
                                {
                                    Priority = CacheItemPriority.NeverRemove,
                                    AbsoluteExpiration = absoluteExpiration
                                });
                            }

                            break;
                        }
                    }
                }
            }
        }
    }
}