using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public struct ChangeTokenInfo
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

    public static class FileProviderHelper
    {
        public static string NormalizePath(string filter) => filter = filter.Replace('\\', '/');

        public static void CancelToken(ChangeTokenInfo matchInfo)
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

        public static IChangeToken GetOrAddFileChangeToken(string fullname, ConcurrentDictionary<string, ChangeTokenInfo> fileTokenLookup)
        {
            var pattern = NormalizePath(fullname);

            ChangeTokenInfo tokenInfo;
            if (!fileTokenLookup.TryGetValue(fullname, out tokenInfo))
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken);
                tokenInfo = fileTokenLookup.GetOrAdd(fullname, tokenInfo);
            }

            IChangeToken changeToken = tokenInfo.ChangeToken;
            return changeToken;
        }
    }
}
