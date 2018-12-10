﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NetCoreStack.Hisar;

namespace Admin.Tabler.Hosting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<DefaultHisarStartup<Startup>>();
    }
}
