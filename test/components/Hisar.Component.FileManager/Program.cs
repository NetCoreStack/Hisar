using System.IO;
using Microsoft.AspNetCore.Hosting;
using NetCoreStack.Hisar;

namespace Hisar.Component.FileManager
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
