using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Threading;
using Microsoft.Extensions.FileSystemGlobbing;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public class DefaultCliFileLocator : IDefaultCliFileLocator
    {
        private struct ChangeTokenInfo
        {
            public ChangeTokenInfo(
                CancellationTokenSource tokenSource,
                CancellationChangeToken changeToken)
                : this(tokenSource, changeToken, matcher: null)
            {
            }

            public ChangeTokenInfo(
                CancellationTokenSource tokenSource,
                CancellationChangeToken changeToken,
                Matcher matcher)
            {
                TokenSource = tokenSource;
                ChangeToken = changeToken;
                Matcher = matcher;
            }

            public CancellationTokenSource TokenSource { get; }

            public CancellationChangeToken ChangeToken { get; }

            public Matcher Matcher { get; }
        }

        private readonly ConcurrentDictionary<string, ChangeTokenInfo> _fileTokenLookup = 
            new ConcurrentDictionary<string, ChangeTokenInfo>(StringComparer.OrdinalIgnoreCase);

        private static string NormalizePath(string filter) => filter = filter.Replace('\\', '/');

        public IFileInfo Layout { get; set; }

        public DefaultCliFileLocator()
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes("<html><head><body>@RenderBody()</body></head></html>")))
            {
                Layout = new InMemoryFileInfo("_Layout.cshtml", "/Views/Shared/_Layout.cshtml", memoryStream.ToArray(), DateTime.UtcNow);
            }
        }

        private static void CancelToken(ChangeTokenInfo matchInfo)
        {
            if (matchInfo.TokenSource.IsCancellationRequested)
            {
                return;
            }

            Task.Run(() =>
            {
                try
                {
                    matchInfo.TokenSource.Cancel();
                }
                catch
                {
                }
            });
        }

        private IChangeToken GetOrAddFileChangeToken(string fullname)
        {
            ChangeTokenInfo tokenInfo;
            if (!_fileTokenLookup.TryGetValue(fullname, out tokenInfo))
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken);
                tokenInfo = _fileTokenLookup.GetOrAdd(fullname, tokenInfo);
            }

            IChangeToken changeToken = tokenInfo.ChangeToken;
            return changeToken;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var name = Path.GetFileName(subpath);
            return new NotFoundFileInfo(name);
        }

        public IChangeToken CreateFileChangeToken(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            pattern = NormalizePath(pattern);
            var changeToken = GetOrAddFileChangeToken(pattern);
            return changeToken;
        }

        public void RaiseChange(string fullname)
        {
            fullname = NormalizePath(fullname);
            ChangeTokenInfo matchInfo;
            if (_fileTokenLookup.TryRemove(fullname, out matchInfo))
            {
                CancelToken(matchInfo);
            }
        }
    }
}