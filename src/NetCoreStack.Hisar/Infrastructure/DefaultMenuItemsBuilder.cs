using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class DefaultMenuItemsBuilder<TStartup> : IMenuItemsBuilder
    {
        public RunningComponentHelper Component => new RunningComponentHelperOfT<TStartup>(ComponentTypeResolver);
        public string ComponentId => typeof(TStartup).GetTypeInfo().Assembly.GetComponentId();
        
        public IComponentTypeResolver ComponentTypeResolver { get; }

        public DefaultMenuItemsBuilder(IComponentTypeResolver componentTypeResolver)
        {
            ComponentTypeResolver = componentTypeResolver;
        }

        public string ResolvePath(IUrlHelper urlHelper, string path)
        {
            return ComponentHelperBase.ResolveContentPath(urlHelper, ComponentId, path);
        }

        public virtual IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            return new List<IMenuItem>();
        }
    }
}
