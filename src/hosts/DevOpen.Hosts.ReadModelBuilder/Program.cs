using System;
using System.Threading.Tasks;
using DevOpen.Infrastructure.Persistence.EventStore;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpen.Hosts.ReadModelBuilder
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = Bootstrapper.GetServiceProvider();
            var connectionProvider = serviceProvider.GetRequiredService<IEventStoreConnectionProvider>();

            var subscription = serviceProvider.GetRequiredService<ReadModelBuilderSubscription>();

            await connectionProvider.Connect();

            Console.ReadLine();
            
            subscription.StopSubscription();

            Console.ReadLine();
        }
    }
}