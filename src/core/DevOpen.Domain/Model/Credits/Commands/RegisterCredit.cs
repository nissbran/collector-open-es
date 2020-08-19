using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.Domain.Model.Credits.Commands
{
    public class RegisterCredit : Command
    {
        public CreditId Id { get; }
        
        public LoanApplicationId? ApplicationId { get; }
        
        public OrganisationNumber OrganisationNumber { get; set; }
        
        public Address InvoiceAddress { get; set; } = Address.Empty;
        
        public Money LoanAmount { get; set; }
        
        public long? CreditNumber { get; set; }

        public RegisterCredit(CreditId id, LoanApplicationId? applicationId = null)
        {
            Id = id;
            ApplicationId = applicationId;
        }
    }
}