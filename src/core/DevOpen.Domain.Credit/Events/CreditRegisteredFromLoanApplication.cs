using DevOpen.Domain.Model;

namespace DevOpen.Domain.Events
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