using System.Runtime.Serialization;

namespace DevOpen.Domain.Events
{
    public abstract class LoanApplicationDomainEvent : DomainEvent
    {
        [IgnoreDataMember]
        public override string AggregateType => "LoanApplication";
    }
}