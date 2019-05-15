using System;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public class CreditRootRepository : ICreditRootRepository
    {
        private readonly IEventStore _eventStore;

        public CreditRootRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<Credit> GetById(CreditId creditId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new CreditEventStreamId(creditId));

            if (domainEvents.Count == 0)
                return null;

            return new Credit(creditId, domainEvents.Cast<CreditDomainEvent>());
        }

        public async Task Save(Credit credit)
        {
            foreach (var uncommittedEvent in credit.UncommittedEvents)
            {
                uncommittedEvent.AggregateId = credit.Id;
                uncommittedEvent.Occurred = DateTimeOffset.Now;
            }
            
            await _eventStore.SaveEvents(new CreditEventStreamId(credit.Id), credit.GetExpectedVersion(), credit.UncommittedEvents);
        }
    }
}