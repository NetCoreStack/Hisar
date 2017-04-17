using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetCoreStack.Hisar
{
    public class DefaultFileProvider : IFileProvider
    {
        public static string LayoutFullName => "/Views/Shared/_Layout.cshtml";

        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != '/' && c != '\\').ToArray();
        
        private readonly IDefaultLayoutFileProvider _layoutFileProvider;
        public DefaultFileProvider(IDefaultLayoutFileProvider layoutFileProvider)
        {
            _layoutFileProvider = layoutFileProvider;
        }

        private static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(_invalidFileNameChars) != -1;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new EnumerableDirectoryContents(new List<IFileInfo>());
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (subpath == LayoutFullName)
            {
                return _layoutFileProvider.Layout;
            }

            var name = Path.GetFileName(subpath);
            return new NotFoundFileInfo(name);
        }

        public IChangeToken Watch(string filter)
        {
            return _layoutFileProvider.CreateFileChangeToken(filter);
        }
    }
}
