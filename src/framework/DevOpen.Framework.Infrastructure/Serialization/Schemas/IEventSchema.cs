using System;
using System.Collections.Generic;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Infrastructure.Serialization.Schemas
{
    public interface IEventSchema
    {
        string Name { get; }

        Type GetDomainEventType(string eventType);

        EventType GetEventType(DomainEvent domainEvent);
        
        IReadOnlyCollection<string> EventTypes { get; }
    }
}