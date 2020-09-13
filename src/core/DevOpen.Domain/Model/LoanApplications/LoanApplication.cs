using System.Collections.Generic;
using DevOpen.Domain.Model.LoanApplications.Commands;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Domain.Model.LoanApplications.Projections;
using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.LoanApplications
{
    public class LoanApplication : AggregateRoot<LoanApplicationId, LoanApplicationState>
    {
        public LoanApplicationId Id => State.Id;
        
        public LoanApplication(LoanApplicationId id, IEnumerable<LoanApplicationDomainEvent> historicEvents) : 
            base(new LoanApplicationState(id, historicEvents))
        {
        }
        
        public LoanApplication(LoanApplicationId id, RegisterLoanApplication cmd, long creditNumber) : 
            base(new LoanApplicationState(id))
        {
            ApplyChange(new LoanApplicationRegistered(cmd.OrganisationNumber, cmd.RequestedAmount, creditNumber));
            
            if (!cmd.VisitingAddress.IsEmpty)
                ApplyChange(new VisitingAddressRegistered(cmd.VisitingAddress));
            if (!cmd.InvoiceAddress.IsEmpty)
                ApplyChange(new InvoiceAddressRegistered(cmd.InvoiceAddress));
        }

        public void Approve()
        {
            ApplyChange(new ApplicationApproved());
        }

        public void Deny()
        {
            ApplyChange(new ApplicationDenied());
        }
    }
}