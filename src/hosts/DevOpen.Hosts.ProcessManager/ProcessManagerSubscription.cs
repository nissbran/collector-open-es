using System;
using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Persistence.Sql;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Subscriptions;
using EventStore.ClientAPI;

namespace DevOpen.Hosts.ProcessManager
{
    public class ProcessManagerSubscription : EventStoreCatchUpAllSubscriber
    {
        private static readonly Guid SubscriptionCheckpointId = Guid.Parse("12efbb94-d23d-4c7a-bda3-ca5d389b17a3");
        
        private readonly IEventSerializer _eventSerializer;
        private readonly ProcessManagerMediator _mediator;
        private readonly SubscriptionCheckpointStorage _checkpointStorage;
        private readonly SubscriptionCheckpoint _subscriptionCheckpoint;

        public ProcessManagerSubscription(
            IEventStoreConnectionProvider connectionProvider,
            IEventSerializer eventSerializer,
            ProcessManagerMediator mediator,
            SubscriptionCheckpointStorage checkpointStorage) : base("ProcessManager", connectionProvider.Connection, new CatchAllSubscriptionSettings())
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