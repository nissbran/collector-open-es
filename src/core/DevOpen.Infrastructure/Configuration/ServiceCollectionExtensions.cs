using DevOpen.Application.Handlers;
using DevOpen.Application.Handlers.Commands;
using DevOpen.Application.Handlers.Queries;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Repositories;
using DevOpen.Infrastructure.Repositories.Aggregates;
using DevOpen.Infrastructure.Repositories.Views;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Serialization.Schemas;
using DevOpen.ReadModel.LoanApplications;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpen.Infrastructure.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<CommandMediator>();
            services.AddSingleton<QueryMediator>();
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ICommandHandler))
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler)))
                .AsImplementedInterfaces()
            );
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IQueryHandler))
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler)))
                .AsImplementedInterfaces()
            );
        }
        
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoanApplicationRepository, LoanApplicationRootRepository>();
            services.AddSingleton<ILoanApplicationViewRepository, LoanApplicationViewRepository>();
            services.AddSingleton<ICreditRootRepository, CreditRootRepository>();
        }
        
        public static void AddEventStore(this IServiceCollection services)
        {
            services.AddSingleton<IEventSerializer>(provider => new JsonEventSerializer(
                new IEventSchema[]
                {
                    new LoanApplicationSchema(), 
                    new CreditSchema()
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