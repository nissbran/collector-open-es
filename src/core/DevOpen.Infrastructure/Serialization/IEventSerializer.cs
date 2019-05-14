using System;
using DevOpen.Domain;
using EventStore.ClientAPI;

namespace DevOpen.Infrastructure.Serialization
{
    public interface IEventSerializer
    {
        bool CanHandleEventType(string eventType);
        EventData SerializeDomainEvent(Guid commitId, DomainEvent domainEvent);
        DomainEvent DeserializeEvent(ResolvedEvent resolvedEvent);
    }
}