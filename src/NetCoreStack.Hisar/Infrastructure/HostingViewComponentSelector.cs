using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    /// <summary>
    /// Hisar implementation of <see cref="IViewComponentSelector"/>.
    /// </summary>
    public class HostingViewComponentSelector : IViewComponentSelector
    {
        private readonly IViewComponentDescriptorCollectionProvider _descriptorProvider;
        private readonly HisarAssemblyComponentsLoader _assemblyLoader;

        private ViewComponentDescriptorCache _cache;

        /// <summary>
        /// Creates a new <see cref="HostingViewComponentSelector"/>.
        /// </summary>
        /// <param name="descriptorProvider">The <see cref="IViewComponentDescriptorCollectionProvider"/>.</param>
        /// <param name="assemblyLoader">The <see cref="HisarAssemblyComponentsLoader"/>.</param>
        public HostingViewComponentSelector(IViewComponentDescriptorCollectionProvider descriptorProvider, 
            HisarAssemblyComponentsLoader assemblyLoader)
        {
            _descriptorProvider = descriptorProvider;
            _assemblyLoader = assemblyLoader;
        }

        /// <inheritdoc />
        public ViewComponentDescriptor SelectComponent(string componentName)
        {
            if (componentName == null)
            {
                throw new ArgumentNullException(nameof(componentName));
            }

            var collection = _descriptorProvider.ViewComponents;
            if (_cache == null || _cache.Version != collection.Version)
            {
                _cache = new ViewComponentDescriptorCache(collection);
            }

            var partDefinition = _assemblyLoader.EnsureIsComponentPart(componentName);
            if (partDefinition != null)
            {
                return _cache.SelectFromComponent(partDefinition.Assembly, partDefinition.Name);
            }

            // ViewComponent names can either be fully-qualified, or refer to the 'short-name'. If the provided
            // name contains a '.' - then it's a fully-qualified name.
            if (componentName.Contains("."))
            {
                return _cache.SelectByFullName(componentName);
            }
            else
            {
                return _cache.SelectByShortName(componentName);
            }
        }

        private class ViewComponentDescriptorCache
        {
            private readonly ILookup<string, ViewComponentDescriptor> _lookupByShortName;
            private readonly ILookup<string, ViewComponentDescriptor> _lookupByFullName;
            private readonly ILookup<Assembly, ViewComponentDescriptor> _lookupByAssembly;

            public ViewComponentDescriptorCache(ViewComponentDescriptorCollection collection)
            {
                Version = collection.Version;

                _lookupByShortName = collection.Items.ToLookup(c => c.ShortName, c => c);
                _lookupByFullName = collection.Items.ToLookup(c => c.FullName, c => c);
                _lookupByAssembly = collection.Items.ToLookup(c => c.TypeInfo.Assembly, c => c);
            }

            public int Version { get; }

            public ViewComponentDescriptor SelectFromComponent(Assembly componentAssembly, string name)
            {
                if (componentAssembly == null)
                {
                    throw new ArgumentNullException(nameof(componentAssembly));
                }

                var matches = _lookupByAssembly[componentAssembly];
                var count = matches.Count();
                if (count == 0)
                {
                    return null;
                }

                return matches.FirstOrDefault(x => x.ShortName == name);
            }

            public ViewComponentDescriptor SelectByShortName(string name)
            {
                return Select(_lookupByShortName, name);
            }

            public ViewComponentDescriptor SelectByFullName(string name)
            {
                return Select(_lookupByFullName, name);
            }

            private static ViewComponentDescriptor Select(
                ILookup<string, ViewComponentDescriptor> candidates,
                string name)
            {
                var matches = candidates[name];

                var count = matches.Count();
                if (count == 0)
                {
                    return null;
                }
                else if (count == 1)
                {
                    return matches.Single();
                }
                else
                {
                    var matchedTypes = new List<string>();
                    foreach (var candidate in matches)
                    {
                        matchedTypes.Add(string.Format("AmbiguousTypeMatch {0}, {1}",
                            candidate.TypeInfo.FullName,
                            candidate.FullName));
                    }

                    var typeNames = string.Join(Environment.NewLine, matchedTypes);
                    throw new InvalidOperationException(string.Format("AmbiguousTypeMatch {0} {1} {2}", name, Environment.NewLine, typeNames));
                }
            }
        }
    }
}
