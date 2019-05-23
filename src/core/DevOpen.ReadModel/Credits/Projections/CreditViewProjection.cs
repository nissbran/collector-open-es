using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.ReadModel.Credits.Projections
{
    public class CreditViewProjection
    {
        public void Apply(CreditViewModel model, CreditDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case CreditRegistered creditRegistered:
                    model.LoanAmount = creditRegistered.LoanAmount;
                    model.OrganisationNumber = creditRegistered.OrganisationNumber;
                    model.Balance = Money.Create(0, creditRegistered.LoanAmount.Currency);
                    break;
                
                case CreditRegisteredFromLoanApplication fromLoanApplication:
                    model.ApplicationId = fromLoanApplication.LoanApplicationId;
                    break;
                
                case DisbursementRegistered disbursementRegistered:
                    model.Balance -= disbursementRegistered.Amount;
                    break;
            }
        }
    }
}