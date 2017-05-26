using System;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public static class AssemblyExtensions
    {
        public static string GetComponentId(this Assembly componentAssembly)
        {
            return componentAssembly.GetName().Name.Split('.').Last();
        }

        public static bool EnsureIsHosting(this Assembly assembly)
        {
            var componentId = assembly.GetComponentId();
            if (componentId.Equals(EngineConstants.HostingComponentName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        internal static ComponentPartDefinition EnsureIsComponentPart(this HisarAssemblyComponentsLoader lookup, string componentName)
        {
            if (lookup == null)
            {
                throw new ArgumentNullException(nameof(lookup));
            }

            if (string.IsNullOrEmpty(componentName))
            {
                throw new ArgumentNullException(nameof(componentName));
            }
            
            if (componentName.StartsWith(HisarAssemblyComponentsLoader.ComponentConventionBaseNamespace))
            {
                var token = componentName.Split('.');
                var componentId = token[2];
                var name = token.Last();

                lookup.ComponentAssemblyLookup.TryGetValue(componentId, out Assembly componentAssembly);
                if (componentAssembly == null)
                    return null;

                return new ComponentPartDefinition
                {
                    ComponentId = componentId,
                    Name = name,
                    Assembly = componentAssembly
                };
            }
            else
            {
                var token = componentName.Split('.');
                if (token.Length == 2)
                {
                    var componentId = token.First();
                    var name = token.Last();

                    lookup.ComponentAssemblyLookup.TryGetValue(componentId, out Assembly componentAssembly);
                    if (componentAssembly == null)
                        return null;

                    return new ComponentPartDefinition
                    {
                        ComponentId = componentId,
                        Name = name,
                        Assembly = componentAssembly
                    };
                }
            }

            return null;
        }
    }
}