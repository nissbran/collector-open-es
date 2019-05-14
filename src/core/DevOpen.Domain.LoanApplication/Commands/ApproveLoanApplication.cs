using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class ApproveLoanApplication : Command
    {
        public LoanApplicationId Id { get; }
        
        public ApproveLoanApplication(LoanApplicationId applicationId)
        {
            Id = applicationId;
        }
    }
}