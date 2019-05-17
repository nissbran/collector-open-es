using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Subscriptions;
using EventStore.ClientAPI;

namespace DevOpen.Hosts.ReadModelBuilder
{
    public class ReadModelBuilderSubscription : EventStoreCatchUpAllSubscriber
    {
        private readonly IEventSerializer _eventSerializer;
        private readonly ReadModelBuilderMediator _mediator;

        public ReadModelBuilderSubscription(
            IEventStoreConnectionProvider connectionProvider,
            IEventSerializer eventSerializer,
            ReadModelBuilderMediator mediator) : base("ReadModelBuilder", connectionProvider.Connection, new CatchAllSubscriptionSettings())
        {
            _eventSerializer = eventSerializer;
            _mediator = mediator;
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
    }
}