using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders.Embedded;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.FileProviders;
using System.Collections.Concurrent;

namespace NetCoreStack.Hisar
{

    /// <summary>
    /// I have written this to workaround a present issue: https://github.com/aspnet/FileSystem/issues/184
    /// </summary>
    public class DirectoryFriendlyEmbeddedFileProvider : IFileProvider
    {

        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars().Where(c => c != '/' && c != '\\').ToArray();
        private readonly Assembly _assembly;
        private readonly string _baseNamespace;
        private readonly DateTimeOffset _lastModified;
        private readonly ConcurrentDictionary<string, EmbeddedResourceFileInfo> _fileLookupCache = new ConcurrentDictionary<string, EmbeddedResourceFileInfo>();

        /// <summary> 
        /// Initializes a new instance of the <see cref="EmbeddedFileProvider" /> class using the specified 
        /// assembly and empty base namespace. 
        /// </summary> 
        /// <param name="assembly"></param> 
        public DirectoryFriendlyEmbeddedFileProvider(Assembly assembly)
            : this(assembly, assembly?.GetName()?.Name)
        {
        }

        /// <summary> 
        /// Initializes a new instance of the <see cref="EmbeddedFileProvider" /> class using the specified 
        /// assembly and base namespace. 
        /// </summary> 
        /// <param name="assembly">The assembly that contains the embedded resources.</param> 
        /// <param name="baseNamespace">The base namespace that contains the embedded resources.</param> 
        public DirectoryFriendlyEmbeddedFileProvider(Assembly assembly, string baseNamespace)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            _baseNamespace = string.IsNullOrEmpty(baseNamespace) ? string.Empty : baseNamespace + ".";
            _assembly = assembly;
            _lastModified = DateTimeOffset.UtcNow;

        }


        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            var name = Path.GetFileName(subpath);

            var encodedPath = EncodeAsResourcesPath(subpath);
            var resourcePath = _baseNamespace + encodedPath;

            if(_fileLookupCache.TryGetValue(resourcePath, out EmbeddedResourceFileInfo fileInfo))
            {
                return fileInfo;
            }

            if (HasInvalidPathChars(resourcePath))
            {
                return new NotFoundFileInfo(resourcePath);
            }

            if (_assembly.GetManifestResourceInfo(resourcePath) == null)
            {
                return new NotFoundFileInfo(name);
            }

            fileInfo = new EmbeddedResourceFileInfo(_assembly, resourcePath, name, _lastModified);
            _fileLookupCache.TryAdd(resourcePath, fileInfo);
            return fileInfo;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {

            // The file name is assumed to be the remainder of the resource name. 
            if (subpath == null)
            {
                return new NotFoundDirectoryContents();
            }

            var encodedPath = EncodeAsResourcesPath(subpath);
            var resourcePath = _baseNamespace + encodedPath;
            if (!resourcePath.EndsWith("."))
            {
                resourcePath = resourcePath + ".";
            }
            var entries = new List<IFileInfo>();

            // We will assume that any file starting with this path, is in that directory.
            // NOTE: This may include false positives, but helps in the majority of cases until 
            // https://github.com/aspnet/FileSystem/issues/184 is solved.

            // TODO: The list of resources in an assembly isn't going to change. Consider caching. 
            var resources = _assembly.GetManifestResourceNames();
            for (var i = 0; i < resources.Length; i++)
            {
                var resourceName = resources[i];
                if (resourceName.StartsWith(resourcePath))
                {
                    entries.Add(new EmbeddedResourceFileInfo(
                        _assembly,
                        resourceName,
                        resourceName.Substring(resourcePath.Length),
                        _lastModified));
                }
            }


            return new EnumerableDirectoryContents(entries);


        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }

        protected virtual string EncodeAsResourcesPath(string subPath)
        {

            var builder = new StringBuilder(subPath.Length);

            // does the subpath contain directory portion - if so we need to encode it.
            var indexOfLastSeperator = subPath.LastIndexOf('/');
            if (indexOfLastSeperator != -1)
            {
                // has directory portion to encode.
                for (int i = 0; i <= indexOfLastSeperator; i++)
                {
                    var currentChar = subPath[i];

                    if (currentChar == '/')
                    {
                        if (i != 0) // omit a starting slash (/), encode any others as a dot
                        {
                            builder.Append('.');
                        }
                        continue;
                    }

                    if (currentChar == '-')
                    {
                        builder.Append('_');
                        continue;
                    }

                    builder.Append(currentChar);
                }
            }

            // now append (and encode as necessary) filename portion
            if (subPath.Length > indexOfLastSeperator + 1)
            {
                // has filename to encode
                for (int c = indexOfLastSeperator + 1; c < subPath.Length; c++)
                {
                    var currentChar = subPath[c];
                    // no encoding to do on filename - so just append
                    builder.Append(currentChar);
                }
            }

            return builder.ToString();

        }

        private static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(_invalidFileNameChars) != -1;
        }


    }
}