using System.Collections.Generic;
using System.Linq;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.Credits.Projections
{
    public class CreditState : AggregateState<CreditId>
    {
        internal Money LoanAmount { get; private set; }
        
        internal Money Balance { get; private set; }
        
        internal Address InvoiceAddress { get; private set; }
        
        private readonly List<Disbursement> _disbursements = new List<Disbursement>();
        
        public CreditState(CreditId id) : base(id)
        {
        }

        public CreditState(CreditId id, IEnumerable<CreditDomainEvent> historicEvents) : base(id)
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
                case CreditRegistered creditRegistered:
                    LoanAmount = creditRegistered.LoanAmount;
                    Balance = Money.Create(0, creditRegistered.LoanAmount.Currency);
                    break;
                
                case DisbursementRegistered disbursementRegistered:
                    _disbursements.Add(new Disbursement(disbursementRegistered.DisbursementId, disbursementRegistered.Amount));
                    break;
            }
        }
    }
}