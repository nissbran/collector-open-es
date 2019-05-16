using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.Domain.Model.Credits.Events
{
    [EventType("CreditRegisteredFromLoanApplication")]
    public class CreditRegisteredFromLoanApplication : CreditDomainEvent
    {
        public LoanApplicationId LoanApplicationId { get; }

        public CreditRegisteredFromLoanApplication(LoanApplicationId loanApplicationId)
        {
            LoanApplicationId = loanApplicationId;
        }
    }
}