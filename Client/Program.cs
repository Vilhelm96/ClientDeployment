using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Client
{
    /// <summary>
    /// This is the starter-kit that we use in the training course IdentityServer in Production  
    /// by https://www.tn-data.se
    /// </summary>
    public class Program
    {
        private static readonly string _applicationName = "Client";

        public static int Main(string[] args)
        {
            Settings.StartupTime = DateTime.Now;

            try
            {
                Log.Information("Starting web host for " + _applicationName);
                CreateHostBuilder(args)
                    .ConfigureSerilog(_applicationName, options =>
                    {
                        ConfigureLogLevels(options);
                    })
                    .UseSerilog()
                    .Build()
                    .Run();
                return 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        private static void ConfigureLogLevels(LoggerConfiguration options)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            switch (environmentName)
            {
                case "Production":
                    options.MinimumLevel.Override("System", LogEventLevel.Warning)
                           .MinimumLevel.Override("IdentityServer4", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", LogEventLevel.Debug)
                           .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Information);
                    break;
                case "Offline":
                    options.MinimumLevel.Override("System", LogEventLevel.Information)
                           .MinimumLevel.Override("IdentityServer4", LogEventLevel.Debug)
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                           .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", LogEventLevel.Debug)
                           .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Information);
                    break;
                default:
                    //Development
                    options.MinimumLevel.Override("System", LogEventLevel.Warning)
                           .MinimumLevel.Override("IdentityServer4", LogEventLevel.Debug)
                           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                           .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.AspNetCore.DataProtection", LogEventLevel.Debug)
                           .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                           .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Information);
                    break;
            }
        }
    }
}
