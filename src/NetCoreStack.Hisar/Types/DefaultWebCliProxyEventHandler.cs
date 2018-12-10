using NetCoreStack.WebSockets;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;

namespace NetCoreStack.Hisar
{
    public class DefaultWebCliProxyEventHandler : IWebCliProxyEventHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultWebCliProxyEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task FileChanged(FileChangedContext context)
        {
            var connectionManager = _serviceProvider.GetService<IConnectionManager>();
            if (connectionManager != null)
            {
                await connectionManager.BroadcastAsync(new WebSocketMessageContext
                {
                    Header = new RouteValueDictionary(new { fileChanged = true }),
                    Command = WebSocketCommands.DataSend,
                    Value = context
                });
            }
        }
    }
}
