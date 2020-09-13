using System;
using DevOpen.Framework.Domain;
using EventStore.ClientAPI;

namespace DevOpen.Framework.Infrastructure.Serialization
{
    public interface IEventSerializer
    {
        bool CanHandleEventType(string eventType);
        EventData SerializeDomainEvent(Guid commitId, DomainEvent domainEvent);
        DomainEvent DeserializeEvent(ResolvedEvent resolvedEvent);
    }
}