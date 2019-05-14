using System;
using DevOpen.Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpen.Hosts.Api
{
    internal static class Bootstrapper
    {
        private static readonly ServiceProvider ServiceProvider;
        
        static Bootstrapper()
        {
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