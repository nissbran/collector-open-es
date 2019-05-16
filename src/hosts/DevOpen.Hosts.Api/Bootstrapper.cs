using System;
using DevOpen.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DevOpen.Hosts.Api
{
    internal static class Bootstrapper
    {
        private static readonly ServiceProvider ServiceProvider;
        
        static Bootstrapper()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            
            var services = new ServiceCollection();
            
            services.AddApplicationServices();
            services.AddInfrastructureServices();
            
            services.AddEventStore();

            ServiceProvider = services.BuildServiceProvider();
        }
        
        public static IServiceProvider GetServiceProvider()
        {
            return ServiceProvider;
        }
    }
}