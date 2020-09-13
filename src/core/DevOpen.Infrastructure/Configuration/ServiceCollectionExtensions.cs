using System;
using DevOpen.Application.Handlers;
using DevOpen.Application.Handlers.Commands;
using DevOpen.Application.Handlers.Queries;
using DevOpen.Application.Processes;
using DevOpen.Application.Repositories;
using DevOpen.Framework.Application.Handlers;
using DevOpen.Framework.Application.Mediators;
using DevOpen.Framework.Application.Processes;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.Framework.Infrastructure.Persistence.EventStoreDb;
using DevOpen.Framework.Infrastructure.Serialization;
using DevOpen.Framework.Infrastructure.Serialization.Schemas;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.Infrastructure.Persistence.Hybrid;
using DevOpen.Infrastructure.Persistence.InMemory;
using DevOpen.Infrastructure.Persistence.Sql;
using DevOpen.Infrastructure.Repositories.Aggregates;
using DevOpen.Infrastructure.Repositories.Views;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Serialization.Schemas;
using DevOpen.Infrastructure.Storage;
using DevOpen.ReadModel;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.Search;
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
                .FromAssembliesOf(typeof(ApplicationLayerAssembly))
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler)))
                .AsImplementedInterfaces()
            );
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ApplicationLayerAssembly))
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler)))
                .AsImplementedInterfaces()
            );
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ApplicationLayerAssembly))
                .AddClasses(classes => classes.AssignableTo(typeof(IProcessManager)))
                .AsImplementedInterfaces()
            );
            
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(ReadModelLayerAssembly))
                .AddClasses(classes => classes.AssignableTo(typeof(IReadModelBuilder)))
                .AsImplementedInterfaces()
            );
        }
        
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoanApplicationAggregateStore, LoanApplicationAggregateStore>();
            services.AddSingleton<IApplicationLiveProjectionRepository, ApplicationLiveProjectionRepository>();
            services.AddSingleton<ICreditAggregateStore, CreditAggregateStore>();
            services.AddSingleton<ICreditLiveProjectionRepository, CreditLiveProjectionRepository>();
            
            services.AddSingleton<ICreditNumberRepository, CreditNumberSqlStorage>();

            services.AddSingleton<ICreditLookup, CreditSqlLookup>();
            services.AddSingleton<ISearchViewStore, SearchViewElasticStore>();
            
            services.AddSingleton<ElasticIndexNameResolver>();
            services.AddSingleton<ElasticConnectionProvider>();
            
            services.AddSingleton<SubscriptionCheckpointStorage>();
            services.AddSingleton<IProcessedEventsStorage, ProcessedEventsSqlStorage>();
        }

        public static void AddSwitchableViewStores(this IServiceCollection services, Guid subscriptionCheckpointId)
        {
            var subscriptionCheckpoint = new SubscriptionCheckpointStorage().Load(subscriptionCheckpointId);
            if (subscriptionCheckpoint.IsInStartPosition)
            {
                services.AddSingleton<CreditInMemoryViewStore>();
                services.AddSingleton<CreditViewElasticStore>();
                services.AddSingleton<ICreditViewStore, CreditHybridViewStore>();
                
                services.AddSingleton<ApplicationInMemoryViewStore>();
                services.AddSingleton<ApplicationViewElasticStore>();
                services.AddSingleton<IApplicationViewStore, ApplicationHybridViewStore>();
            }
            else
            {
                services.AddViewStores();
            }
        }

        public static void AddViewStores(this IServiceCollection services)
        {
            services.AddSingleton<ICreditViewStore, CreditViewElasticStore>();
            services.AddSingleton<IApplicationViewStore, ApplicationViewElasticStore>();
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