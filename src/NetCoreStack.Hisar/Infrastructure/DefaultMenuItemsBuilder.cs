using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    public class DefaultMenuItemsBuilder<TStartup> : IMenuItemsBuilder
    {
        public RunningComponentHelper Component { get; }

        public DefaultMenuItemsBuilder()
        {
            Component = new RunningComponentHelperOfT<TStartup>(new DefaultComponentTypeResolver());
        }

        public string ResolvePath(IUrlHelper urlHelper, string path)
        {
            return ComponentHelperBase.ResolveContentPath(urlHelper, Component.ComponentId, path);
        }

        public virtual IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            return new List<IMenuItem>();
        }
    }
}
