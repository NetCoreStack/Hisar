using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

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

        private RunningComponentHelper _componentHelper;
        protected RunningComponentHelper ComponentHelper
        {
            get
            {
                if (_componentHelper == null)
                    _componentHelper = Resolver.GetService<RunningComponentHelper>();

                return _componentHelper;
            }
        }

        protected string ExecutionComponentId { get; }

        protected string ComponentName { get; }

        public HisarViewComponent()
        {
            var componentType = GetType().GetTypeInfo();

            var attribute = componentType.GetCustomAttribute<ViewComponentAttribute>();
            if (!string.IsNullOrEmpty(attribute?.Name))
                ComponentName = attribute.Name;
            else
                ComponentName = ViewComponentConventions.GetComponentName(componentType);
            
            ExecutionComponentId = componentType.Assembly.GetComponentId();
        }

        protected virtual TInstance GetService<TInstance>()
        {
            return Resolver.GetService<TInstance>();
        }

        public new ViewViewComponentResult View()
        {
            if (ComponentHelper.IsExternalComponent)
            {
                return base.View();
            }

            return base.View($"{ExecutionComponentId}.Default");
        }

        public new ViewViewComponentResult View<TModel>(TModel model)
        {
            if (ComponentHelper.IsExternalComponent)
            {
                return base.View(model);
            }

            return base.View($"{ExecutionComponentId}.Default", model);
        }

        public new ViewViewComponentResult View(string viewName)
        {
            if (ComponentHelper.IsExternalComponent)
            {
                return base.View(viewName);
            }

            return base.View($"{ExecutionComponentId}.{viewName}");
        }
    }
}
