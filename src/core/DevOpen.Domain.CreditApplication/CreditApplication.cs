using System.Collections.Generic;
using DevOpen.Domain.Commands;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Domain.Projections;

namespace DevOpen.Domain
{
    public class CreditApplication : AggregateRoot<CreditApplicationId, CreditApplicationStateProjection>
    {
        public CreditApplicationId Id => State.Id;

        private CreditApplication(CreditApplicationStateProjection state) : base(state)
        {
        }

        public CreditApplication(CreditApplicationId id, IEnumerable<CreditApplicationDomainEvent> historicEvents) : this(new CreditApplicationStateProjection(id, historicEvents))
        {
        }
        
        public CreditApplication(CreditApplicationId id, RegisterCreditApplication cmd) : this(new CreditApplicationStateProjection(id))
        {
            ApplyChange(new CreditApplicationRegistered(cmd.OrganisationNumber, cmd.RequestedAmount));
            
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