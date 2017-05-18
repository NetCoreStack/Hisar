using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Template
{
    public class CustomMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public CustomMenuBuilder(IComponentTypeResolver componentTypeResolver) 
            : base(componentTypeResolver)
        {
        }

        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            return new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Text = "Registration",
                    Path = ResolvePath(urlHelper, "~/Home/Registration"),
                    ShowInMenu = true
                }
            };
        }
    }
}
