using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.LoanApplications.Commands
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