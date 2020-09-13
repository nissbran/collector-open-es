using System;

namespace DevOpen.Framework.Infrastructure.Serialization
{
    public class DomainMetadata
    {
        public string AggregateRootId { get; set; }

        public DateTimeOffset Occurred { get; set; }

        public Guid CorrelationId { get; set; }

        public string Schema { get; set; }

        public int EventTypeVersion { get; set; }
    }
}