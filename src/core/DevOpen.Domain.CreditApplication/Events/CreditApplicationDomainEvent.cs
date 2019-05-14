using System.Runtime.Serialization;

namespace DevOpen.Domain.Events
{
    public abstract class CreditApplicationDomainEvent : DomainEvent
    {
        [IgnoreDataMember]
        public override string AggregateType => "CreditApplication";
    }
}