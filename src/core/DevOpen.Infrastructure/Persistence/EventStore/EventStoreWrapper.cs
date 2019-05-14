using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Infrastructure.Serialization;
using EventStore.ClientAPI;

namespace DevOpen.Infrastructure.Persistence.EventStore
{
    public class EventStoreWrapper : IEventStore
    {
        private const int ReadBatchSize = 200;

        private readonly IEventStoreConnection _connection;
        private readonly IEventSerializer _eventSerializer;

        public EventStoreWrapper(IEventStoreConnection connection, IEventSerializer eventSerializer)
        {
            _connection = connection;
            _eventSerializer = eventSerializer;
        }

        public async Task<IList<DomainEvent>> GetEventsByStreamId(EventStreamId eventStreamId)
        {
            return await GetEventsFromStreamVersion(eventStreamId.StreamName, StreamPosition.Start, eventStreamId.ResolveLinks);
        }

        private async Task<IList<DomainEvent>> GetEventsFromStreamVersion(string streamName, long streamVersion, bool resolveLinks)
        {
            var streamEvents = new List<ResolvedEvent>();

            StreamEventsSlice currentSlice;
            var nextSliceStart = streamVersion;
            do
            {
                currentSlice = await _connection.ReadStreamEventsForwardAsync(
                    stream: streamName,
                    start: nextSliceStart,
                    count: ReadBatchSize,
                    resolveLinkTos: resolveLinks);

                nextSliceStart = currentSlice.NextEventNumber;

                streamEvents.AddRange(currentSlice.Events);

            } while (!currentSlice.IsEndOfStream);

            return streamEvents.Select(ConvertEventDataToDomainEvent).ToList();
        }

        public async Task<StreamWriteResult> SaveEvents(EventStreamId eventStreamId, long streamVersion, List<DomainEvent> events)
        {
            if (events.Any() == false)
                return new StreamWriteResult(-1);

            var commitId = Guid.NewGuid();

            var expectedVersion = streamVersion == 0 ? ExpectedVersion.NoStream : streamVersion - 1;
            var eventsToSave = events.Select(domainEvent => ToEventData(commitId, domainEvent)).ToList();

            var result = await _connection.AppendToStreamAsync(
                stream: eventStreamId.ToString(),
                expectedVersion: expectedVersion,
                events: eventsToSave);

            return new StreamWriteResult(result.NextExpectedVersion);
        }

        private DomainEvent ConvertEventDataToDomainEvent(ResolvedEvent resolvedEvent)
        {
            return _eventSerializer.DeserializeEvent(resolvedEvent);
        }

        private EventData ToEventData(Guid commitId, DomainEvent domainEvent)
        {
            return _eventSerializer.SerializeDomainEvent(commitId, domainEvent);
        }
    }
}