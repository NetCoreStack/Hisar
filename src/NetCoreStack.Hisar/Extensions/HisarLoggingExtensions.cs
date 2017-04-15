using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace NetCoreStack.Hisar
{
    public static class HisarLoggingExtensions
    {
        public static async Task LogDbAsync(this ILogger logger, EventId eventId, Exception ex, string message, params object[] args)
        {
            await Task.CompletedTask;
            logger.LogCritical(eventId, ex, message, args);
        }
    }
}