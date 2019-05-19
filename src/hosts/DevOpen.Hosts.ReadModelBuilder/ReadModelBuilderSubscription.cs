using System;
using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Persistence.Sql;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Subscriptions;
using EventStore.ClientAPI;

namespace DevOpen.Hosts.ReadModelBuilder
{
    public class ReadModelBuilderSubscription : EventStoreCatchUpAllSubscriber
    {
        private static readonly Guid SubscriptionCheckpointId = Guid.Parse("7c511bd6-04e8-40f6-b07a-94bf9e23009b");
        
        private readonly IEventSerializer _eventSerializer;
        private readonly ReadModelBuilderMediator _mediator;
        private readonly SubscriptionCheckpointStorage _checkpointStorage;
        private readonly SubscriptionCheckpoint _subscriptionCheckpoint;

        public ReadModelBuilderSubscription(
            IEventStoreConnectionProvider connectionProvider,
            IEventSerializer eventSerializer,
            ReadModelBuilderMediator mediator,
            SubscriptionCheckpointStorage checkpointStorage) : base("ReadModelBuilder", connectionProvider.Connection, new CatchAllSubscriptionSettings())
        {
            _eventSerializer = eventSerializer;
            _mediator = mediator;
            _checkpointStorage = checkpointStorage;
            
            _subscriptionCheckpoint = _checkpointStorage.Load(SubscriptionCheckpointId);
            
            LastCommitPosition = _subscriptionCheckpoint.LastProcessedPosition;
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
        }
        
        protected override async Task OnUpdateLastCommitPosition(long newPosition)
        {
            _subscriptionCheckpoint.IncrementEventsProcessed();
            _subscriptionCheckpoint.SetLastProcessedPosition(newPosition);
            await _checkpointStorage.Save(_subscriptionCheckpoint);
            LastCommitPosition = newPosition;
        }
    }
}