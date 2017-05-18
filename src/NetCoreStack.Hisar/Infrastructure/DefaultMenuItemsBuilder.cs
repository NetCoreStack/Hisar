using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class DefaultMenuItemsBuilder<TStartup> : IMenuItemsBuilder
    {
        public RunningComponentHelper Component => new RunningComponentHelperOfT<TStartup>(ComponentTypeResolver);
        public string ComponentId => typeof(TStartup).GetTypeInfo().Assembly.GetComponentId();

        public IUrlHelperFactory UrlHelperFactory { get; }
        public IComponentTypeResolver ComponentTypeResolver { get; }

        public DefaultMenuItemsBuilder(IUrlHelperFactory urlHelperFactory, IComponentTypeResolver componentTypeResolver)
        {
            ComponentTypeResolver = componentTypeResolver;
            UrlHelperFactory = urlHelperFactory;
        }

        public virtual IEnumerable<IMenuItem> Build(ActionContext context)
        {
            return new List<IMenuItem>();
        }
    }
}
