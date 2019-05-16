namespace DevOpen.Domain.Model.LoanApplications.Commands
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