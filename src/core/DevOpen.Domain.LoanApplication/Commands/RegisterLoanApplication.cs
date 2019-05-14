using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class RegisterLoanApplication : Command
    {
        public LoanApplicationId Id { get; }
        
        public OrganisationNumber OrganisationNumber { get; set; }
        
        public Address VisitingAddress { get; set; } = Address.Empty;
        
        public Address InvoiceAddress { get; set; } = Address.Empty;
        
        public Money RequestedAmount { get; set; }

        public RegisterLoanApplication(LoanApplicationId applicationId)
        {
            Id = applicationId;
        }
    }
}