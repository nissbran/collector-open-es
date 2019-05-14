using System.Collections.Generic;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;

namespace DevOpen.ReadModel.LoanApplications
{
    public class LoanApplicationView
    {
        public Money RequestedAmount { get; }
        
        public OrganisationNumber OrganisationNumber { get; }
        
        public LoanApplicationStatus Status { get; }

        public LoanApplicationView(IEnumerable<LoanApplicationDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                switch (domainEvent)
                {
                    case LoanApplicationRegistered applicationRegistered:
                        RequestedAmount = applicationRegistered.RequestedAmount;
                        OrganisationNumber = applicationRegistered.OrganisationNumber;
                        Status = LoanApplicationStatus.Registered;
                        break;
                
                    case ApplicationApproved _:
                        Status = LoanApplicationStatus.Approved;
                        break;
                    
                    case ApplicationDenied _:
                        Status = LoanApplicationStatus.Denied;
                        break;
                }
            }
        }
    }
}