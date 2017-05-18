using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NetCoreStack.Hisar;
using System.Collections.Generic;

namespace Hisar.Component.Carousel
{
    public class CarouselMenuBuilder : DefaultMenuItemsBuilder<Startup>
    {
        public CarouselMenuBuilder(IUrlHelperFactory urlHelperFactory, IComponentTypeResolver componentTypeResolver) 
            : base(urlHelperFactory, componentTypeResolver)
        {
        }

        public override IEnumerable<IMenuItem> Build(ActionContext context)
        {
            return base.Build(context);
        }
    }
}
