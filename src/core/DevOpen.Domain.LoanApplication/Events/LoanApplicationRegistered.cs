using DevOpen.Domain.Model;

namespace DevOpen.Domain.Events
{
    [EventType("CreditApplicationRegistered")]
    public class LoanApplicationRegistered : LoanApplicationDomainEvent
    {
        public OrganisationNumber OrganisationNumber { get; }
        
        public Money RequestedAmount { get; }
        
        public LoanApplicationRegistered(OrganisationNumber organisationNumber, Money requestedAmount)
        {
            OrganisationNumber = organisationNumber;
            RequestedAmount = requestedAmount;
        }
    }
}