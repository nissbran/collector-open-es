using DevOpen.Application.Handlers.Commands;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories.Aggregates;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Repositories;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Serialization.Schemas;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpen.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<CommandMediator>();
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ICommandHandler))
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler)))
                .AsImplementedInterfaces()
            );
        }
        
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            
            
            services.AddSingleton<ICreditApplicationRepository, CreditApplicationRootRepository>();
            //services.AddSingleton<ICreditRootRepository, CreditRootRepository>();
        }
        
        public static void AddEventStore(this IServiceCollection services)
        {
            services.AddSingleton<IEventSerializer>(provider => new JsonEventSerializer(
                new IEventSchema[]
                {
                    new CreditApplicationSchema(), 
                    //new CreditSchema(),
                }));
            
            services.AddSingleton<IEventStore>(provider =>
            {
                var connection = EventStoreConnectionFactory.Create(
                    "ConnectTo=tcp://localhost:1113",
                    "admin", "changeit");

                connection.ConnectAsync().Wait();
                
                return new EventStoreWrapper(connection, provider.GetRequiredService<IEventSerializer>());
            });
        }
    }
}