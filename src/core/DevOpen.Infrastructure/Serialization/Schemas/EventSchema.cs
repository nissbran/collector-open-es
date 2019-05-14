using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevOpen.Domain;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public abstract class EventSchema<TBaseEvent> : IEventSchema where TBaseEvent : DomainEvent
    {
        private readonly HashSet<string> _eventTypes = new HashSet<string>();
        private readonly Dictionary<string, Type> _definitionToType = new Dictionary<string, Type>();
        private readonly Dictionary<Type, EventType> _typeToDefinition = new Dictionary<Type, EventType>();

        public abstract string Name { get; }

        public IReadOnlyCollection<string> EventTypes => _eventTypes;

        protected EventSchema()
        {
            var baseEvent = typeof(TBaseEvent);
            var types = baseEvent.GetTypeInfo().Assembly.GetTypes()
                .Where(p => baseEvent.IsAssignableFrom(p));

            foreach (var type in types)
            {
                if (type.GetTypeInfo().GetCustomAttribute(typeof(EventTypeAttribute)) is EventTypeAttribute eventType)
                {
                    _eventTypes.Add(eventType.Name);
                    _definitionToType.Add(new EventType(eventType.Name, eventType.Version), type);
                    _typeToDefinition.Add(type, new EventType(eventType.Name, eventType.Version));
                }
            }
        }

        public Type GetDomainEventType(string eventType)
        {
            var eventDefinition = new EventType(eventType);

            if (_definitionToType.TryGetValue(eventDefinition.EventName, out var domainEvent))
                return domainEvent;

            return null;
        }

        public EventType GetEventType(DomainEvent domainEvent)
        {
            if (_typeToDefinition.TryGetValue(domainEvent.GetType(), out var eventDefinition))
                return eventDefinition;

            throw new NotImplementedException();
        }
    }
}