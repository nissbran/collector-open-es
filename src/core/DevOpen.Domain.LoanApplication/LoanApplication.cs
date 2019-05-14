using System.Collections.Generic;
using DevOpen.Domain.Commands;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Domain.Projections;

namespace DevOpen.Domain
{
    public class LoanApplication : AggregateRoot<LoanApplicationId, LoanApplicationStateProjection>
    {
        public LoanApplicationId Id => State.Id;

        private LoanApplication(LoanApplicationStateProjection state) : base(state)
        {
        }

        public LoanApplication(LoanApplicationId id, IEnumerable<LoanApplicationDomainEvent> historicEvents) : this(new LoanApplicationStateProjection(id, historicEvents))
        {
        }
        
        public LoanApplication(LoanApplicationId id, RegisterLoanApplication cmd) : this(new LoanApplicationStateProjection(id))
        {
            ApplyChange(new LoanApplicationRegistered(cmd.OrganisationNumber, cmd.RequestedAmount));
            
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