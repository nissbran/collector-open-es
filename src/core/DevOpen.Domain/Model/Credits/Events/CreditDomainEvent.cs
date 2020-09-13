using System.Runtime.Serialization;
using DevOpen.Framework.Domain;

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