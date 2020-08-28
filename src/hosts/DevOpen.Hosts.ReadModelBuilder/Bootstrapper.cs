using System;
using DevOpen.Infrastructure.Configuration;
using DevOpen.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DevOpen.Hosts.ReadModelBuilder
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
            services.AddSwitchableViewStores(ReadModelBuilderSubscription.SubscriptionCheckpointId);
            
            services.AddEventStore();

            services.AddSingleton<ReadModelBuilderSubscription>();
            services.AddSingleton<RebuildCoordinator>();

            ServiceProvider = services.BuildServiceProvider();
        }
        
        public static IServiceProvider GetServiceProvider()
        {
            return ServiceProvider;
        }
    }
}