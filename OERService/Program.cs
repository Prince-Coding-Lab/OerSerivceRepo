using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace OERService
{
    public class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
              .UseStartup<Startup>();
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(Configuration)
                   .CreateLogger();
            Log.Information("OER Service is running");

        }
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddEnvironmentVariables()
              .Build();    
      
    }
}
