namespace DevOpen.Domain.Model.LoanApplications.Events
{
    [EventType("LoanApplicationRegistered")]
    public class LoanApplicationRegistered : LoanApplicationDomainEvent
    {
        public OrganisationNumber OrganisationNumber { get; }
        
        public Money RequestedAmount { get; }
        public long CreditNumber { get; }
        
        public LoanApplicationRegistered(OrganisationNumber organisationNumber, Money requestedAmount, long creditNumber)
        {
            OrganisationNumber = organisationNumber;
            RequestedAmount = requestedAmount;
            CreditNumber = creditNumber;
        }
    }
}