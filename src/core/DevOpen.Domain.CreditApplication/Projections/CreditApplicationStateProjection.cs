using System.Collections.Generic;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;

namespace DevOpen.Domain.Projections
{
    public class CreditApplicationStateProjection : AggregateState<CreditApplicationId>
    {
        internal Money RequestedAmount { get; private set; }
        
        internal Address VisitingAddress { get; private set; }
        internal Address InvoiceAddress { get; private set; }

        internal CreditApplicationStatus Status { get; private set; } = CreditApplicationStatus.Registered;
        
        public CreditApplicationStateProjection(CreditApplicationId id) : base(id)
        {
        }

        public CreditApplicationStateProjection(CreditApplicationId id, IEnumerable<CreditApplicationDomainEvent> historicEvents) : base(id)
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
                case CreditApplicationRegistered applicationRegistered:
                    Status = CreditApplicationStatus.Registered;
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
                    Status = CreditApplicationStatus.Approved;
                    break;
                    
                case ApplicationDenied _:
                    Status = CreditApplicationStatus.Denied;
                    break;
                    
            }
        }
    }
}