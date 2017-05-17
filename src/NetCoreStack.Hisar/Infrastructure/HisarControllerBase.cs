using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public abstract class HisarControllerBase : Controller
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

        private Lazy<IDataProtectorProvider> _dataProtector;
        public Lazy<IDataProtectorProvider> DataProtector
        {
            get
            {
                if (_dataProtector == null)
                {
                    _dataProtector = new Lazy<IDataProtectorProvider>(() => Resolver.GetService<IDataProtectorProvider>());
                }
                return _dataProtector;
            }
        }

        protected virtual TInstance GetService<TInstance>()
        {
            return Resolver.GetService<TInstance>();
        }

        protected virtual JsonResult CreateWebResult(ResultState state, params string[] validations)
        {
            if (validations != null && validations.Any())
            {
                ModelValidationResult result = new ModelValidationResult()
                {
                    Messages = validations.ToList()
                };
                return Json(new WebResult(resultState: state, validations: new List<ModelValidationResult> { result }));
            }

            return Json(new WebResult(resultState: state));
        }
    }
}