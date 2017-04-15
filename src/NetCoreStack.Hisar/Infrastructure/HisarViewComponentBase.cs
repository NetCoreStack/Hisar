using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace NetCoreStack.Hisar
{
    public abstract class HisarViewComponent : ViewComponent
    {
        public IServiceProvider Resolver
        {
            get
            {
                return HttpContext?.RequestServices;
            }
        }

        private ICommonCacheProvider _cacheProvider;
        protected ICommonCacheProvider CacheProvider
        {
            get
            {
                if (_cacheProvider == null)
                    _cacheProvider = Resolver.GetService<ICommonCacheProvider>();

                return _cacheProvider;
            }
        }
    }
}
