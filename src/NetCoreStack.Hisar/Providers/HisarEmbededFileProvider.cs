using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetCoreStack.Hisar
{
    public class HisarEmbededFileProvider : IFileProvider
    {
        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != '/' && c != '\\').ToArray();

        private readonly IDictionary<string, Assembly> _componentsAssemblyLookup;
        private readonly DateTimeOffset _lastModified;

        public HisarEmbededFileProvider(IDictionary<string, Assembly> componentsAssemblyLookup)
        {
            _componentsAssemblyLookup = componentsAssemblyLookup;
            _lastModified = DateTimeOffset.UtcNow;
        }

        private string GetBaseNameSpace(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var baseNamespace = assembly?.GetName()?.Name;
            return string.IsNullOrEmpty(baseNamespace) ? string.Empty : baseNamespace + ".";
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var name = Path.GetFileName(subpath);
            var nameTree = name.Split('.');
            if (nameTree.Length > 2)
            {
                var componentId = nameTree[0];
                var viewName = nameTree[1];
                var extension = nameTree[2];

                var componentViewName = string.Empty;
                if (_componentsAssemblyLookup.TryGetValue(componentId, out Assembly componentAssembly))
                {
                    var baseNamespace = GetBaseNameSpace(componentAssembly);

                    var builder = new StringBuilder(baseNamespace.Length + subpath.Length);
                    builder.Append(baseNamespace);

                    var directoryName = Path.GetDirectoryName(subpath);
                    subpath = Path.Combine(directoryName, $"{viewName}.{extension}");

                    // Relative paths starting with a leading slash okay
                    if (subpath.StartsWith("/", StringComparison.Ordinal) || subpath.StartsWith("\\", StringComparison.Ordinal))
                    {
                        builder.Append(subpath, 1, subpath.Length - 1);
                    }
                    else
                    {
                        builder.Append(subpath);
                    }

                    for (var i = baseNamespace.Length; i < builder.Length; i++)
                    {
                        if (builder[i] == '/' || builder[i] == '\\')
                        {
                            builder[i] = '.';
                        }
                    }

                    var resourcePath = builder.ToString();
                    if (HasInvalidPathChars(resourcePath))
                    {
                        return new NotFoundFileInfo(resourcePath);
                    }

                    if (componentAssembly.GetManifestResourceInfo(resourcePath) == null)
                    {
                        return new NotFoundFileInfo(name);
                    }

                    return new EmbeddedResourceFileInfo(componentAssembly, resourcePath, name, _lastModified);
                }
            }

            return new NotFoundFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            // The file name is assumed to be the remainder of the resource name.
            if (subpath == null)
            {
                return NotFoundDirectoryContents.Singleton;
            }

            // Relative paths starting with a leading slash okay
            if (subpath.StartsWith("/", StringComparison.Ordinal))
            {
                subpath = subpath.Substring(1);
            }

            // Non-hierarchal.
            if (!subpath.Equals(string.Empty))
            {
                return NotFoundDirectoryContents.Singleton;
            }

            var entries = new List<IFileInfo>();
            return new EnumerableDirectoryContents(entries);
        }

        /// <summary>
        /// Embedded files do not change.
        /// </summary>
        /// <param name="pattern">This parameter is ignored</param>
        /// <returns>A <see cref="NullChangeToken" /></returns>
        public IChangeToken Watch(string pattern)
        {
            return NullChangeToken.Singleton;
        }

        private static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(_invalidFileNameChars) != -1;
        }
    }
}