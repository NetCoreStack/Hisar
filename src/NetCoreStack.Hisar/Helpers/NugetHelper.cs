using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;

namespace NetCoreStack.Hisar
{
    internal static class NugetHelper
    {
        private static IDictionary<string, List<string>> _targetFrameworkMap =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["netcoreapp"] = new List<string> { "netstandard1.6", "netstandard1.3", "netstandard1.1" },
                ["netcoreapp1.1"] = new List<string> { "netcoreapp1.1" }
            };

        private static string NugetDownloadPackageUriFormat = "https://www.nuget.org/api/v2/package/{0}/{1}";
        private static HttpClient _client = new HttpClient();

        private static Assembly LoadCandidateAssembly(string packageId, 
            string extractFullPath, 
            List<string> candidateCompiledTarget)
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
                            return AssemblyLoadContext.Default.LoadFromAssemblyPath(packageDll.FullName);
                        }
                    }
                }
            }

            return null;
        }

        public static Assembly TryLoadFromNuget(string externalComponentsRefDirectory,
            string targetFrameworkName,
            string packageId,
            string version,
            bool deleteMetadataFolders = true)
        {
            _targetFrameworkMap.TryGetValue(targetFrameworkName, out List<string> candidateCompiledTarget);
            var extractFullPath = Path.Combine(externalComponentsRefDirectory, $"{packageId}.{version}");
            if (Directory.Exists(extractFullPath))
            {
                return LoadCandidateAssembly(packageId, extractFullPath, candidateCompiledTarget);
            }

            var requestUri = new Uri(string.Format(NugetDownloadPackageUriFormat, packageId, version));
            var response = _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri)).GetAwaiter().GetResult();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var packageBytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                var packageFullName = Path.Combine(externalComponentsRefDirectory, $"{packageId}.{version}.nupkg");
                File.WriteAllBytes(packageFullName, packageBytes);

                ZipArchive archive = new ZipArchive(new FileStream(packageFullName, FileMode.Open));
                Directory.CreateDirectory(extractFullPath);
                archive.ExtractToDirectory(extractFullPath);

                if (deleteMetadataFolders)
                {
                    string[] filePaths = Directory.GetFiles(extractFullPath);
                    foreach (string filePath in filePaths)
                    {
                        var filename = Path.GetFileName(filePath);
                        File.Delete(filePath);
                    }

                    string[] directories = Directory.GetDirectories(extractFullPath);
                    foreach (string directory in directories)
                    {
                        var directoryName = Path.GetFileName(directory);
                        if (directoryName != "lib")
                        {
                            Directory.Delete(directory, true);
                        }
                    }
                }

                return LoadCandidateAssembly(packageId, extractFullPath, candidateCompiledTarget);
            }
            else
            {
                throw new FileLoadException($"{packageId}.{version} could not be loaded! [Nuget Source]");
            }
        }
    }
}
