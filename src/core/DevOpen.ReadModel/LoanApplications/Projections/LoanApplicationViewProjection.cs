using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.ReadModel.LoanApplications.Projections
{
    public class LoanApplicationViewProjection
    {
        public void Apply(LoanApplicationViewModel model, LoanApplicationDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case LoanApplicationRegistered applicationRegistered:
                    model.RequestedAmount = applicationRegistered.RequestedAmount;
                    model.OrganisationNumber = applicationRegistered.OrganisationNumber;
                    model.Status = LoanApplicationStatus.Registered;
                    break;
                
                case ApplicationApproved _:
                    model.Status = LoanApplicationStatus.Approved;
                    break;
                    
                case ApplicationDenied _:
                    model.Status = LoanApplicationStatus.Denied;
                    break;
            }
        }
    }
}