using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.Credits.Events
{
    [EventType("CreditRegistered")]
    public class CreditRegistered : CreditDomainEvent
    {
        public OrganisationNumber OrganisationNumber { get; }
        
        public Money LoanAmount { get; }
        
        public long CreditNumber { get; }
        
        public CreditRegistered(OrganisationNumber organisationNumber, Money loanAmount, long creditNumber)
        {
            OrganisationNumber = organisationNumber;
            LoanAmount = loanAmount;
            CreditNumber = creditNumber;
        }
    }
}