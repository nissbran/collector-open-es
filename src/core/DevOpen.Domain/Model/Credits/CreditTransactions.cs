using System.Collections.Generic;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Domain.Model.Credits.Projections;

namespace DevOpen.Domain.Model.Credits
{
    public class CreditTransactions : AggregateRoot<CreditId, CreditTransactionState>
    {
        public CreditId Id => State.Id;
        
        public CreditTransactions(CreditId creditId) : base(new CreditTransactionState(creditId))
        {
        }

        public CreditTransactions(CreditId creditId, IEnumerable<CreditDomainEvent> historicEvents) : base(new CreditTransactionState(creditId, historicEvents))
        {
            throw new System.NotImplementedException();
        }
    }
}