using System;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Mediators;
using DevOpen.Framework.Infrastructure.Persistence.EventStoreDb;
using DevOpen.Framework.Infrastructure.Serialization;
using DevOpen.Framework.Infrastructure.Subscriptions;
using DevOpen.Infrastructure.Persistence;
using DevOpen.Infrastructure.Persistence.Sql;
using DevOpen.Infrastructure.Serialization;
using EventStore.ClientAPI;
using Serilog;

namespace DevOpen.Hosts.ReadModelBuilder
{
    public class ReadModelBuilderSubscription : EventStoreCatchUpAllSubscriber
    {
        public static readonly Guid SubscriptionCheckpointId = Guid.Parse("7c511bd6-04e8-40f6-b07a-94bf9e23009b");
        
        private readonly IEventSerializer _eventSerializer;
        private readonly ReadModelBuilderMediator _mediator;
        private readonly RebuildCoordinator _rebuildCoordinator;
        private readonly SubscriptionCheckpointStorage _checkpointStorage;
        private readonly SubscriptionCheckpoint _subscriptionCheckpoint;

        public ReadModelBuilderSubscription(
            IEventStoreConnectionProvider connectionProvider,
            IEventSerializer eventSerializer,
            ReadModelBuilderMediator mediator,
            RebuildCoordinator rebuildCoordinator,
            SubscriptionCheckpointStorage checkpointStorage) : base("ReadModelBuilder", connectionProvider.Connection, new CatchAllSubscriptionSettings())
        {
            _eventSerializer = eventSerializer;
            _mediator = mediator;
            _rebuildCoordinator = rebuildCoordinator;
            _checkpointStorage = checkpointStorage;
            
            _subscriptionCheckpoint = _checkpointStorage.Load(SubscriptionCheckpointId);

            if (_subscriptionCheckpoint.IsInStartPosition)
            {
                _rebuildCoordinator.ClearModels();
            }
            
            LastCommitPosition = _subscriptionCheckpoint.LastProcessedPosition;
        }
        
        protected override void BeforeSubscriptionEpochStarts()
        {
            if (_subscriptionCheckpoint.IsInStartPosition)
            {
                _rebuildCoordinator.StartRebuild();
            }
        }
        
        protected override bool IsValidEvent(ResolvedEvent resolvedEvent)
        {
            return resolvedEvent.Event != null &&
                   !resolvedEvent.Event.EventType.StartsWith("$") &&
                   _eventSerializer.CanHandleEventType(resolvedEvent.Event.EventType);
        }
        
        protected override async Task EventAppeared(ResolvedEvent resolvedEvent, long currentPosition)
        {
            var domainEvent = _eventSerializer.DeserializeEvent(resolvedEvent);

            await _mediator.MediateEvent(domainEvent);
            
            if (_rebuildCoordinator.IsRebuilding)
            {
                _rebuildCoordinator.ReportRebuildProgress(_subscriptionCheckpoint.EventsProcessed);
            }
        }
        
        protected override async Task OnUpdateLastCommitPosition(long newPosition)
        {
            _subscriptionCheckpoint.IncrementEventsProcessed();
            _subscriptionCheckpoint.SetLastProcessedPosition(newPosition);
            await _checkpointStorage.Save(_subscriptionCheckpoint);
            LastCommitPosition = newPosition;
        }

        protected override void LiveProcessingStarted(EventStoreCatchUpSubscription subscription)
        {
            if (_rebuildCoordinator.IsRebuilding)
            {
                _rebuildCoordinator.FinishRebuild();
            }

            Log.Information("Live processing started!");
        }
    }
}