using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;

namespace NetCoreStack.Hisar
{
    public class HisarViewComponentSelector : IViewComponentSelector
    {
        private readonly RunningComponentHelper _helper;

        public HisarViewComponentSelector(RunningComponentHelper helper)
        {
            _helper = helper;
        }

        public ViewComponentDescriptor SelectComponent(string componentName)
        {
            throw new NotImplementedException();
        }
    }
}
