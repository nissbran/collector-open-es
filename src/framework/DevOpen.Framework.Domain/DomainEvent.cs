using System;
using System.Runtime.Serialization;

namespace DevOpen.Framework.Domain
{
    public abstract class DomainEvent
    {   
        public abstract string AggregateType { get; }
        
        [IgnoreDataMember]
        public string AggregateId { get; set; }

        [IgnoreDataMember]
        public long EventNumber { get; set; }
        
        [IgnoreDataMember]
        public DateTimeOffset Occurred { get; set; }
    }
}