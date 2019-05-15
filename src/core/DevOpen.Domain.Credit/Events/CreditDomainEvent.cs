using System.Runtime.Serialization;

namespace DevOpen.Domain.Events
{
    public abstract class CreditDomainEvent : DomainEvent
    {
        [IgnoreDataMember]
        public override string AggregateType => "Credit";
    }
}