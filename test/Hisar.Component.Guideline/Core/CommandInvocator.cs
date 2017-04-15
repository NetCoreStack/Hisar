using NetCoreStack.WebSockets;
using NetCoreStack.WebSockets.ProxyClient;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Hisar.Component.Guideline.Core
{
    public class CommandInvocator : IClientWebSocketCommandInvocator
    {
        public CommandInvocator()
        {

        }

        public async Task InvokeAsync(WebSocketMessageContext context)
        {
            if (context.MessageType == WebSocketMessageType.Binary)
            {
                object pageName = null;
                if (context.Header.TryGetValue("pageupdated", out pageName))
                {
                    var pageContent = context.Value;
                }
            }

            await Task.CompletedTask;
        }
    }
}
