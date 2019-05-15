using System.Collections.Generic;
using DevOpen.Domain.Commands;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Domain.Projections;

namespace DevOpen.Domain
{
    public class LoanApplication : AggregateRoot<LoanApplicationId, LoanApplicationState>
    {
        public LoanApplicationId Id => State.Id;

        private LoanApplication(LoanApplicationState state) : base(state)
        {
        }

        public LoanApplication(LoanApplicationId id, IEnumerable<LoanApplicationDomainEvent> historicEvents) : this(new LoanApplicationState(id, historicEvents))
        {
        }
        
        public LoanApplication(LoanApplicationId id, RegisterLoanApplication cmd) : this(new LoanApplicationState(id))
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