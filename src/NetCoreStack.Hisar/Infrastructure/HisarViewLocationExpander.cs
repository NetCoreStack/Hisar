using Microsoft.AspNetCore.Mvc.Razor;
using System.Collections.Generic;
using System.Reflection;

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

            if (!string.IsNullOrEmpty(context.AreaName))
            {
                List<string> customViewLocations = new List<string>(viewLocations);
                foreach (KeyValuePair<string, Assembly> entry in _assemblyLoader.ComponentAssemblyLookup)
                {
                    var namespaceExpander = entry.Value.GetName().Name.Replace(".", "/");
                    customViewLocations.Add("/" + entry.Key + "/Views/{1}/{0}.cshtml");
                    customViewLocations.Add("/" + entry.Key + "/Views/Shared/{0}.cshtml");
                    customViewLocations.Add("/" + entry.Key + "/Views/{1}/{0}.cshtml");
                }

                return customViewLocations;
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }
    }
}
