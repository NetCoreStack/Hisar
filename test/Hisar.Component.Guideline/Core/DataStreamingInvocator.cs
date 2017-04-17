using Microsoft.AspNetCore.Routing;
using NetCoreStack.Hisar;
using NetCoreStack.WebSockets;
using NetCoreStack.WebSockets.ProxyClient;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Hisar.Component.Guideline.Core
{
    public class DataStreamingInvocator : IClientWebSocketCommandInvocator
    {
        private readonly IWebSocketConnector _connector;
        private readonly IDefaultLayoutFileProvider _layoutFileProvider;
        public DataStreamingInvocator(IWebSocketConnector connector, IDefaultLayoutFileProvider layoutFileProvider)
        {
            _connector = connector;
            _layoutFileProvider = layoutFileProvider;
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
                object key = null;
                if (context.Header.TryGetValue("pageupdated", out key))
                {
                    var pageContent = context.Value?.ToString();
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(pageContent)))
                    {
                        var fullname = DefaultFileProvider.LayoutFullName;
                        var name = Path.GetFileName(fullname);
                        _layoutFileProvider.Layout = new InMemoryFileInfo(name, fullname, memoryStream.ToArray(), DateTime.UtcNow);
                        _layoutFileProvider.RaiseChange(fullname);
                    }
                }
            }

            await Task.CompletedTask;
        }
    }
}
