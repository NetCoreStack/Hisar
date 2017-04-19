using Microsoft.AspNetCore.Routing;
using NetCoreStack.WebSockets;
using NetCoreStack.WebSockets.ProxyClient;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    internal class DataStreamingInvocator : IClientWebSocketCommandInvocator
    {
        private readonly IWebSocketConnector _connector;
        private readonly IDefaultProxyFileLocator _cliFileProvider;
        public DataStreamingInvocator(IWebSocketConnector connector, IDefaultProxyFileLocator layoutFileProvider)
        {
            _connector = connector;
            _cliFileProvider = layoutFileProvider;
        }

        public async Task InvokeAsync(WebSocketMessageContext context)
        {
            if (context.Command == WebSocketCommands.Handshake)
            {
                var connectionId = context.Value.ToString();
                await _connector.SendAsync(new WebSocketMessageContext
                {
                    Command = WebSocketCommands.DataSend,
                    MessageType = WebSocketMessageType.Text,
                    Value = connectionId,
                    Header = new RouteValueDictionary(new { LayoutRequest = "layoutrequest" })
                });
            }

            if (context.MessageType == WebSocketMessageType.Binary)
            {
                if (context.Header.TryGetValue("fileupdated", out object fullname))
                {
                    var pageContent = context.Value?.ToString();
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(pageContent)))
                    {
                        var fileFullName = fullname.ToString();
                        var name = Path.GetFileName(fileFullName);
                        _cliFileProvider.Layout = new InMemoryFileInfo(name, fileFullName, memoryStream.ToArray(), DateTime.UtcNow);
                        _cliFileProvider.RaiseChange(fileFullName);
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
