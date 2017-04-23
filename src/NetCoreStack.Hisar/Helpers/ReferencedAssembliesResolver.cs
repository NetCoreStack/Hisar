using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;

namespace NetCoreStack.Hisar
{
    internal static class ReferencedAssembliesResolver
    {
        private static string NugetDownloadPackageUriFormat = "https://www.nuget.org/api/v2/package/{0}/{1}";
        private static HttpClient _client = new HttpClient();

        internal static void ResolveAssemblies(string externalComponentsDirectory, Assembly assembly)
        {
            var references = assembly.GetReferencedAssemblies();
            foreach (var reference in references)
            {
                try
                {
                    Assembly.Load(reference);
                }
                catch (Exception ex)
                {
                    TryLoadFromLocal(externalComponentsDirectory, reference.Name, reference.Version.ToString());
                }
            }
        }

        internal static void TryLoadFromLocal(string externalComponentDirectory, string packageId, string version)
        {
            var assemblyFullName = Path.Combine(externalComponentDirectory, $"{packageId}.dll");
            try
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullName);
            }
            catch
            {
                try
                {
                    TryLoadFromNuget(externalComponentDirectory, packageId, version);
                }
                catch
                {
                    throw new FileNotFoundException($"{packageId}.{version} could not be loaded!");
                }
            }
        }

        internal static void TryLoadFromNuget(string externalComponentDirectory, string packageId, string version)
        {
            var requestUri = new Uri(string.Format(NugetDownloadPackageUriFormat, packageId, version));
            var packageBytes = _client.GetByteArrayAsync(requestUri).GetAwaiter().GetResult();
            var packageFullName = Path.Combine(externalComponentDirectory, $"{packageId}.{version}.nupkg");
            File.WriteAllBytes(packageFullName, packageBytes);
        }
    }
}
