using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NetCoreStack.Hisar
{
    public class ComponentFileProvider : IFileProvider
    {
        private readonly Assembly _assembly;
        public ComponentFileProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new EnumerableDirectoryContents(new List<IFileInfo>());
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var name = Path.GetFileName(subpath);
            return new NotFoundFileInfo(name);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}