using DevOpen.Domain.Model;

namespace DevOpen.Domain.Events
{
    [EventType("CreditRegistered")]
    public class CreditRegistered : CreditDomainEvent
    {
        public OrganisationNumber OrganisationNumber { get; }
        
        public Money LoanAmount { get; }
        
        public CreditRegistered(OrganisationNumber organisationNumber, Money loanAmount)
        {
            OrganisationNumber = organisationNumber;
            LoanAmount = loanAmount;
        }
    }
}