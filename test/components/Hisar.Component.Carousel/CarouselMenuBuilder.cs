using Microsoft.AspNetCore.Mvc;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Carousel
{
    public class CarouselMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public CarouselMenuBuilder(IComponentTypeResolver componentTypeResolver) 
            : base(componentTypeResolver)
        {
        }

        public override IEnumerable<IMenuItem> Build(IUrlHelper urlHelper)
        {
            return new List<IMenuItem>
            {
                new DefaultMenuItem
                {
                    Text = "Test",
                    Path = ResolvePath(urlHelper, "~/"),
                    ShowInMenu = true
                }
            };
        }
    }
}
