using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class DenyLoanApplication : Command
    {
        public LoanApplicationId Id { get; }
        
        public DenyLoanApplication(LoanApplicationId applicationId)
        {
            Id = applicationId;
        }
    }
}