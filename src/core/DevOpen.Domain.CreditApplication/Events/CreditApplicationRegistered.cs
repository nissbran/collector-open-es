using DevOpen.Domain.Model;

namespace DevOpen.Domain.Events
{
    [EventType("CreditApplicationRegistered")]
    public class CreditApplicationRegistered : CreditApplicationDomainEvent
    {
        public OrganisationNumber OrganisationNumber { get; }
        
        public Money RequestedAmount { get; }
        
        public CreditApplicationRegistered(OrganisationNumber organisationNumber, Money requestedAmount)
        {
            OrganisationNumber = organisationNumber;
            RequestedAmount = requestedAmount;
        }
    }
}