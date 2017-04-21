using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;

namespace NetCoreStack.Hisar
{
    internal class HisarViewLocationExpander : IViewLocationExpander
    {
        private readonly HisarAssemblyComponentsLoader _assemblyLoader;

        public HisarViewLocationExpander(HisarAssemblyComponentsLoader assemblyLoader)
        {
            _assemblyLoader = assemblyLoader;
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (!string.IsNullOrEmpty(context.ViewName) && context.ViewName.StartsWith("Components/"))
            {
                return viewLocations;
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
