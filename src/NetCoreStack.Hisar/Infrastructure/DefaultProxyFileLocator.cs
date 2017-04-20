using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Text;

namespace NetCoreStack.Hisar
{
    public class DefaultProxyFileLocator : IDefaultProxyFileLocator
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:1444/")
        };

        private readonly ConcurrentDictionary<string, ChangeTokenInfo> _fileTokenLookup = 
            new ConcurrentDictionary<string, ChangeTokenInfo>(StringComparer.OrdinalIgnoreCase);        

        public IFileInfo Layout { get; set; }

        public DefaultProxyFileLocator()
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("<html><head><body>@RenderBody()</body></head></html>")))
            {
                Layout = new InMemoryFileInfo("_Layout.cshtml", "/Views/Shared/_Layout.cshtml", memoryStream.ToArray(), DateTime.UtcNow);
            }
        }

        public IFileInfo GetFileInfo(string fullname)
        {
            var name = Path.GetFileName(fullname);
            var extension = Path.GetExtension(name);
            if (extension == ".map")
            {
                return new NotFoundFileInfo(name);
            }

            var response = _httpClient.GetStringAsync("api/page/getfile?fullname=" + fullname).GetAwaiter().GetResult();
            return new InMemoryFileInfo(name, fullname, Encoding.UTF8.GetBytes(response), DateTime.UtcNow);
        }

        public IChangeToken CreateFileChangeToken(string pattern)
        {
            return FileProviderHelper.GetOrAddFileChangeToken(pattern, _fileTokenLookup);
        }

        public void RaiseChange(string fullname)
        {
            fullname = FileProviderHelper.NormalizePath(fullname);
            ChangeTokenInfo matchInfo;
            if (_fileTokenLookup.TryRemove(fullname, out matchInfo))
            {
                FileProviderHelper.CancelToken(matchInfo);
            }
        }
    }
}