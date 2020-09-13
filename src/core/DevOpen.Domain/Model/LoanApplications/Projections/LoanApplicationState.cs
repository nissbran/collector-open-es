using System.Collections.Generic;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.LoanApplications.Projections
{
    public class LoanApplicationState : AggregateState<LoanApplicationId>
    {
        internal Money RequestedAmount { get; private set; }
        
        internal Address VisitingAddress { get; private set; }
        internal Address InvoiceAddress { get; private set; }

        internal LoanApplicationStatus Status { get; private set; } = LoanApplicationStatus.Registered;
        
        public LoanApplicationState(LoanApplicationId id) : base(id)
        {
        }

        public LoanApplicationState(LoanApplicationId id, IEnumerable<LoanApplicationDomainEvent> historicEvents) : base(id)
        {
            foreach (var historicEvent in historicEvents)
            {
                ApplyEvent(historicEvent);
            }
        }
        
        public sealed override void ApplyEvent(DomainEvent domainEvent)
        {
            Version++;

            switch (domainEvent)
            {
                case LoanApplicationRegistered applicationRegistered:
                    Status = LoanApplicationStatus.Registered;
                    RequestedAmount = applicationRegistered.RequestedAmount;
                    break;
                
                case VisitingAddressRegistered visitingAddressRegistered:
                    VisitingAddress = new Address(
                        visitingAddressRegistered.Street, 
                        visitingAddressRegistered.Street2, 
                        visitingAddressRegistered.PostalCode, 
                        visitingAddressRegistered.City, 
                        visitingAddressRegistered.Country, 
                        visitingAddressRegistered.CareOf);
                    break;
                
                case InvoiceAddressRegistered invoiceAddressRegistered:
                    InvoiceAddress = new Address(
                        invoiceAddressRegistered.Street, 
                        invoiceAddressRegistered.Street2, 
                        invoiceAddressRegistered.PostalCode, 
                        invoiceAddressRegistered.City, 
                        invoiceAddressRegistered.Country, 
                        invoiceAddressRegistered.CareOf);
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