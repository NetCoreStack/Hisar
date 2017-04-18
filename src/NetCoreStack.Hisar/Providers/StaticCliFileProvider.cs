using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace NetCoreStack.Hisar
{
    public class StaticCliFileProvider : IFileProvider
    {
        private static readonly HttpClient _client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:1444")
        };

        public StaticCliFileProvider()
        {
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return new EnumerableDirectoryContents(new List<IFileInfo>());
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var fullname = subpath;
            if (fullname.StartsWith("/"))
            {
                fullname = fullname.Substring(1);
            }

            var name = Path.GetFileName(fullname);
            var response = _client.GetStringAsync("getfile?fullname=" + fullname).GetAwaiter().GetResult();
            return new InMemoryFileInfo(name, subpath, Encoding.UTF8.GetBytes(response), DateTime.UtcNow);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}
