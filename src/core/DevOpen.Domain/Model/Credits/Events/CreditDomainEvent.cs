using System.Runtime.Serialization;

namespace DevOpen.Domain.Model.Credits.Events
{
    public abstract class CreditDomainEvent : DomainEvent
    {
        [IgnoreDataMember]
        public CreditId Id => CreditId.Parse(AggregateId);
        
        [IgnoreDataMember]
        public override string AggregateType => "Credit";
    }
}