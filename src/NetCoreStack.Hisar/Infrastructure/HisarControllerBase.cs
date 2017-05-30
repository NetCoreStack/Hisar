using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NetCoreStack.Contracts;
using NetCoreStack.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public abstract class HisarControllerBase : Controller
    {
        protected virtual TInstance GetService<TInstance>()
        {
            return Resolver.GetService<TInstance>();
        }

        protected virtual JsonResult CreateWebResult(ResultState state, string redirectUrl = "", params string[] validations)
        {
            if (validations != null && validations.Any())
            {
                ModelValidationResult result = new ModelValidationResult()
                {
                    Messages = validations.ToList()
                };
                return Json(new WebResult(resultState: state, validations: new List<ModelValidationResult> { result }));
            }

            var webResult = new WebResult(resultState: state);
            if (!string.IsNullOrEmpty(redirectUrl))
            {
                webResult.RedirectUrl = redirectUrl;
            }

            return Json(webResult);
        }

        public IServiceProvider Resolver
        {
            get
            {
                return HttpContext?.RequestServices;
            }
        }

        private ICommonCacheProvider _cacheProvider;
        public ICommonCacheProvider CacheProvider
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

        protected Action<TModel> TryGetComposer<TModel>()
        {
            return ControllerContext.TryGetComposerInvoker<TModel>();
        }

        protected TModel GetOrCreateCacheItem<TModel>(long id) where TModel : IModelKey<long>
        {
            return CacheProvider.GetOrCreate<TModel>(ControllerContext, id);
        }

        protected TModel GetOrCreateCacheItem<TModel>(string id) where TModel : IModelKey<string>
        {
            return CacheProvider.GetOrCreate<TModel>(ControllerContext, id);
        }

        [NonAction]
        public virtual string ResolveViewName(string name)
        {
            var type = this.GetType();
            return ControllerContext.ResolveViewName(type, name);
        }
    }
}