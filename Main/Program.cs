using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
             .UseWindowsService()
             .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseUrls("https://localhost:44381");
                    webBuilder.UseUrls("http://localhost:23915");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
