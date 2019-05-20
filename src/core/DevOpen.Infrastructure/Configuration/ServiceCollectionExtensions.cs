using DevOpen.Application.Handlers.Commands;
using DevOpen.Application.Handlers.Queries;
using DevOpen.Application.Mediators;
using DevOpen.Application.Processes;
using DevOpen.Application.Repositories;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Persistence.Sql;
using DevOpen.Infrastructure.Repositories.Aggregates;
using DevOpen.Infrastructure.Repositories.Views;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Serialization.Schemas;
using DevOpen.ReadModel;
using DevOpen.ReadModel.Credits;
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
            services.AddSingleton<ProcessManagerMediator>();
            services.AddSingleton<ReadModelBuilderMediator>();
            
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
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IProcessManager))
                .AddClasses(classes => classes.AssignableTo(typeof(IProcessManager)))
                .AsImplementedInterfaces()
            );
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IReadModelBuilder))
                .AddClasses(classes => classes.AssignableTo(typeof(IReadModelBuilder)))
                .AsImplementedInterfaces()
            );
        }
        
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoanApplicationRepository, LoanApplicationRootRepository>();
            services.AddSingleton<ILoanApplicationViewRepository, LoanApplicationViewRepository>();
            services.AddSingleton<ICreditRootRepository, CreditRootRepository>();
            services.AddSingleton<ICreditViewRepository, CreditViewRepository>();

            services.AddSingleton<ICreditLookup, CreditSqlLookup>();
            services.AddSingleton<SubscriptionCheckpointStorage>();
        }
        
        public static void AddEventStore(this IServiceCollection services)
        {
            services.AddSingleton<IEventSerializer>(provider => new JsonEventSerializer(
                new IEventSchema[]
                {
                    new LoanApplicationSchema(), 
                    new CreditSchema()
                }));

            services.AddSingleton<IEventStoreConnectionProvider, EventStoreConnectionProvider>();
            services.AddSingleton<IEventStore, EventStoreWrapper>();
        }
    }
}