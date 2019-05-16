using System.Runtime.Serialization;

namespace DevOpen.Domain.Model.LoanApplications.Events
{
    public abstract class LoanApplicationDomainEvent : DomainEvent
    {
        [IgnoreDataMember] 
        public LoanApplicationId ApplicationId => LoanApplicationId.Parse(AggregateId);
        
        [IgnoreDataMember]
        public override string AggregateType => "LoanApplication";
    }
}