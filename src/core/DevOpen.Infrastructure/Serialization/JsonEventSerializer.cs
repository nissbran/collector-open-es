﻿using System;
using System.Collections.Generic;
using System.Text;
using DevOpen.Domain;
using DevOpen.Infrastructure.Serialization.Resolvers;
using DevOpen.Infrastructure.Serialization.Schemas;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization
{
    public class JsonEventSerializer : IEventSerializer
    {
        private readonly Dictionary<string, IEventSchema> _eventSchemas = new Dictionary<string, IEventSchema>();
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly HashSet<string> _existingEventTypes = new HashSet<string>();

        public JsonEventSerializer(IEnumerable<IEventSchema> eventSchemas)
        {
            foreach (var schema in eventSchemas)
            {
                _eventSchemas.Add(schema.Name, schema);
                _existingEventTypes.UnionWith(schema.EventTypes);
            }
            
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new EventJsonContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public bool CanHandleEventType(string eventType)
        {
            return _existingEventTypes.Contains(eventType);
        }

        public EventData SerializeDomainEvent(Guid commitId, DomainEvent domainEvent)
        {
            _eventSchemas.TryGetValue(domainEvent.AggregateType, out var schema);

            var eventType = schema.GetEventType(domainEvent);
            var eventId = Guid.NewGuid();

            var dataJson = JsonConvert.SerializeObject(domainEvent, _jsonSerializerSettings);
            var metadataJson = JsonConvert.SerializeObject(new DomainMetadata
            {
                CorrelationId = commitId,
                AggregateRootId = domainEvent.AggregateId,
                EventTypeVersion = eventType.LatestVersion,
                Schema = domainEvent.AggregateType,
                Occurred = domainEvent.Occurred
            }, _jsonSerializerSettings);
            
            var data = Encoding.UTF8.GetBytes(dataJson);
            var metadata = Encoding.UTF8.GetBytes(metadataJson);
            
            return new EventData(eventId, eventType, true, data, metadata);
        }

        public DomainEvent DeserializeEvent(ResolvedEvent resolvedEvent)
        {
            var metadataString = Encoding.UTF8.GetString(resolvedEvent.Event.Metadata);
            var eventString = Encoding.UTF8.GetString(resolvedEvent.Event.Data);

            var metadata = JsonConvert.DeserializeObject<DomainMetadata>(metadataString, _jsonSerializerSettings);

            _eventSchemas.TryGetValue(metadata.Schema, out var schema);
            
            var eventType = schema.GetDomainEventType(resolvedEvent.Event.EventType);

            var domainEvent = (DomainEvent)JsonConvert.DeserializeObject(eventString, eventType, _jsonSerializerSettings);
            domainEvent.AggregateId = metadata.AggregateRootId;
            domainEvent.Occurred = metadata.Occurred;
            domainEvent.EventNumber = resolvedEvent.Event.EventNumber;

            return domainEvent;
        }
    }
}