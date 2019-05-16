using System.Runtime.Serialization;

namespace DevOpen.Domain.Model.Credits.Events
{
    public abstract class CreditDomainEvent : DomainEvent
    {
        [IgnoreDataMember]
        public override string AggregateType => "Credit";
    }
}