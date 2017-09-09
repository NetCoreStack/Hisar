using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace NetCoreStack.Hisar
{
    public static class LogHelper
    {
        private static IHostingEnvironment GetHostingEnvironment(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<IHostingEnvironment>(context.HttpContext.RequestServices);
        }

        private static ILoggerFactory GetLoggerFactory(ExceptionContext context)
        {
            return ServiceProviderServiceExtensions.GetService<ILoggerFactory>(context.HttpContext.RequestServices);
        }

        public static void FallbackLog(string message)
        {
            var directory = Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fallback_logs"));
            var logFile = $"log-{DateTime.Now.Date.ToString("ddMMyyyy")}-{Guid.NewGuid()}.txt";
            var fullPath = Path.Combine(directory.FullName, logFile);
            File.AppendAllText(fullPath, message);
        }
    }
}