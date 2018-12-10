using NetCoreStack.WebSockets;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    class AgentCommandInvocator : IServerWebSocketCommandInvocator
    {
        public Task InvokeAsync(WebSocketMessageContext context)
        {
            return Task.CompletedTask;
        }
    }
}
