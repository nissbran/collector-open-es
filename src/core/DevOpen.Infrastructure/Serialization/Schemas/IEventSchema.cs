using System;
using System.Collections.Generic;
using DevOpen.Domain;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public interface IEventSchema
    {
        string Name { get; }

        Type GetDomainEventType(string eventType);

        EventType GetEventType(DomainEvent domainEvent);
        
        IReadOnlyCollection<string> EventTypes { get; }
    }
}