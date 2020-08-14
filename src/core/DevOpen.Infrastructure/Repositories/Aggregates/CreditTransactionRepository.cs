using System;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public class CreditTransactionRepository : ICreditTransactionRepository
    {
        private readonly IEventStore _eventStore;

        public CreditTransactionRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<CreditTransactions> GetById(CreditId creditId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new CreditTransactionsEventStreamId(creditId));

            if (domainEvents.Count == 0)
                return null;

            return new CreditTransactions(creditId, domainEvents.Cast<CreditDomainEvent>());
        }

        public async Task Save(CreditTransactions transactions)
        {
            foreach (var uncommittedEvent in transactions.UncommittedEvents)
            {
                uncommittedEvent.AggregateId = transactions.Id;
                uncommittedEvent.Occurred = DateTimeOffset.Now;
            }
            
            await _eventStore.SaveEvents(new CreditTransactionsEventStreamId(transactions.Id), transactions.GetExpectedVersion(), transactions.UncommittedEvents);
        }
    }
}