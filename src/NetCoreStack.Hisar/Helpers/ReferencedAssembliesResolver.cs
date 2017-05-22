using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.Versioning;

namespace NetCoreStack.Hisar
{
    internal static class ReferencedAssembliesResolver
    {
        private static IDictionary<string, List<string>> _targetFrameworkMap =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["netcoreapp"] = new List<string> { "netstandard1.6", "netstandard1.3", "netstandard1.1" }
            };

        private static string NugetDownloadPackageUriFormat = "https://www.nuget.org/api/v2/package/{0}/{1}";
        private static HttpClient _client = new HttpClient();

        private static void LoadCandidateAssembly(string packageId, string extractFullPath, List<string> candidateCompiledTarget)
        {
            if (candidateCompiledTarget != null && candidateCompiledTarget.Any())
            {
                var directoryInfo = new DirectoryInfo(extractFullPath);
                foreach (var compiledTarget in candidateCompiledTarget)
                {
                    var directories = directoryInfo.GetDirectories($"lib/{compiledTarget}");
                    if (directories.Any())
                    {
                        var libPath = directories.First();
                        var packageDll = libPath.GetFiles($"{packageId}.dll").FirstOrDefault();
                        if (packageDll != null)
                        {
                            AssemblyLoadContext.Default.LoadFromAssemblyPath(packageDll.FullName);
                            break;
                        }
                    }
                }
            }
        }

        private static void TryLoadFromLocal(IAssemblyProviderResolveCallback resolveCallback, 
            string externalComponentDirectory,
            string targetFrameworkName,
            IDictionary<string, string> dependencies,
            string packageId,
            string version)
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
                    if (dependencies.TryGetValue(packageId, out string explicitVersion))
                        version = explicitVersion;

                    TryLoadFromNuget(externalComponentDirectory, targetFrameworkName, packageId, version);
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

        private static void TryLoadFromNuget(string externalComponentDirectory,
            string targetFrameworkName,
            string packageId,
            string version)
        {
            _targetFrameworkMap.TryGetValue(targetFrameworkName, out List<string> candidateCompiledTarget);
            var extractFullPath = Path.Combine(externalComponentDirectory, $"{packageId}.{version}");
            if (Directory.Exists(extractFullPath))
            {
                LoadCandidateAssembly(packageId, extractFullPath, candidateCompiledTarget);
                return;
            }

            var requestUri = new Uri(string.Format(NugetDownloadPackageUriFormat, packageId, version));
            var response = _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri)).GetAwaiter().GetResult();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var packageBytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                var packageFullName = Path.Combine(externalComponentDirectory, $"{packageId}.{version}.nupkg");
                File.WriteAllBytes(packageFullName, packageBytes);

                ZipArchive archive = new ZipArchive(new FileStream(packageFullName, FileMode.Open));
                Directory.CreateDirectory(extractFullPath);
                archive.ExtractToDirectory(extractFullPath);

                LoadCandidateAssembly(packageId, extractFullPath, candidateCompiledTarget);
            }
            else
            {
                throw new FileLoadException($"{packageId}.{version} could not be loaded! [Nuget Source]");
            }
        }

        internal static void ResolveAssemblies(IAssemblyProviderResolveCallback resolveCallback, string externalComponentsDirectory, Assembly assembly)
        {
            var assemblyTargetFramework = assembly.GetCustomAttribute<TargetFrameworkAttribute>();
            var targetFrameworkName = assemblyTargetFramework.FrameworkName.Split(',').First().Substring(1);
            if (string.IsNullOrEmpty(externalComponentsDirectory))
            {
                throw new ArgumentNullException(nameof(externalComponentsDirectory));
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
                        externalComponentsDirectory,
                        targetFrameworkName,
                        dependencies,
                        reference.Name,
                        reference.Version.ToString());
                }
            }
        }
    }
}
