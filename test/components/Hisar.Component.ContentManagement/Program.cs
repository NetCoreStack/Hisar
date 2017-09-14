using Microsoft.AspNetCore.Hosting;
using System.IO;
using NetCoreStack.Hisar;

namespace Hisar.Component.ContentManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<DefaultHisarStartup<Startup>>()
                .Build();

            host.Run();
        }
    }
}
