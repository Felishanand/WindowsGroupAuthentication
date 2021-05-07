using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

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
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseHttpSys();                   
                   
                   webBuilder.ConfigureServices((context, services) =>
                   {
                       services.Configure<HttpSysOptions>(options =>
                       {
                           options.UrlPrefixes.Add(context.Configuration["HttpSysOptions:UrlPrefix"]);
                           options.Authentication.Schemes = AuthenticationSchemes.NTLM;
                           options.Authentication.AllowAnonymous = true;
                           options.MaxConnections = 100;
                           options.MaxRequestBodySize = 30000000;                          
                       });
                   });

                   webBuilder.UseStartup<Startup>();
               })
               //host as window service
               .UseWindowsService();
    
        #region Kestrel Based
        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //     .UseWindowsService()
        //     .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            //webBuilder.UseUrls("https://localhost:44381");
        //            webBuilder.UseUrls("http://localhost:23915");
        //            webBuilder.UseStartup<Startup>();
        //        });
        #endregion
    }
}
