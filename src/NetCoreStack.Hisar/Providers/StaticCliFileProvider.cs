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
            BaseAddress = new Uri("http://localhost:1444/")
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
            var name = Path.GetFileName(fullname);
            var extension = Path.GetExtension(name);
            if (extension == ".map")
            {
                return new NotFoundFileInfo(name);
            }
            
            var response = _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "api/page/getfile?fullname=" + fullname)).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
            {
                return new InMemoryFileInfo(name, subpath, Encoding.UTF8.GetBytes(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()), DateTime.UtcNow);
            }

            return new NotFoundFileInfo(name);
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}
