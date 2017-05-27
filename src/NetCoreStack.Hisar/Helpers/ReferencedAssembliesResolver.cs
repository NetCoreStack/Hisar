using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Versioning;

namespace NetCoreStack.Hisar
{
    internal static class ReferencedAssembliesResolver
    {
        private static void TryLoadFromLocal(IAssemblyProviderResolveCallback resolveCallback, 
            string externalComponentsRefDirectory,
            string targetFrameworkName,
            IDictionary<string, string> dependencies,
            string packageId,
            string version)
        {
            var assemblyFullName = Path.Combine(externalComponentsRefDirectory, $"{packageId}.dll");
            try
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullName);
            }
            catch
            {
                try
                {
                    if (dependencies.TryGetValue(packageId, out string explicitVersion))
                        version = explicitVersion;

                    NugetHelper.TryLoadFromNuget(externalComponentsRefDirectory, targetFrameworkName, packageId, version);
                }
                catch(Exception ex)
                {
                    var entryAssembly = Assembly.GetEntryAssembly();
                    var resolved = resolveCallback.TryLoad(AssemblyLoadContext.Default, entryAssembly, targetFrameworkName, ex);
                    if (!resolved)
                        throw new FileLoadException($"{packageId}.{version} could not be loaded! [Local Source]");
                }
            }
        }

        internal static void ResolveAssemblies(IAssemblyProviderResolveCallback resolveCallback, string externalComponentsRefDirectory, Assembly assembly)
        {
            var assemblyTargetFramework = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
            var targetFrameworkName = assemblyTargetFramework.FrameworkName.Split(',').First().Substring(1);
            if (string.IsNullOrEmpty(externalComponentsRefDirectory))
            {
                throw new ArgumentNullException(nameof(externalComponentsRefDirectory));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var name = assembly.GetName().Name;
            var componentHelper = assembly.GetType($"{name}.ComponentHelper");
            if (componentHelper == null)
            {
                return;
            }

            IDictionary<string, string> dependencies = new Dictionary<string, string>();
            var propInfo = componentHelper.GetProperty("ComponentDependencies");
            if (propInfo != null)
            {
                dependencies = (IDictionary<string, string>)propInfo.GetValue(null);
            }

            var flag = assembly.GetReferencedAssemblies().Any(x => x.Name == "Shared.Library");
            var references = assembly.GetReferencedAssemblies();
            foreach (var reference in references)
            {
                try
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyName(reference);
                }
                catch (Exception ex)
                {
                    TryLoadFromLocal(resolveCallback,
                        externalComponentsRefDirectory,
                        targetFrameworkName,
                        dependencies,
                        reference.Name,
                        reference.Version.ToString());
                }
            }
        }
    }
}
