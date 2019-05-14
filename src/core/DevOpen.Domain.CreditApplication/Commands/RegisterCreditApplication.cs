using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class RegisterCreditApplication : Command
    {
        public CreditApplicationId Id { get; }
        
        public OrganisationNumber OrganisationNumber { get; set; }
        
        public Address VisitingAddress { get; set; } = Address.Empty;
        
        public Address InvoiceAddress { get; set; } = Address.Empty;
        
        public Money RequestedAmount { get; set; }

        public RegisterCreditApplication(CreditApplicationId applicationId)
        {
            Id = applicationId;
        }
    }
}