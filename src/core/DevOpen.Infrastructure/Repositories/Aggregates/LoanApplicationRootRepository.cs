using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public class LoanApplicationRootRepository : ILoanApplicationRepository
    {
        private readonly IEventStore _eventStore;

        public LoanApplicationRootRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<LoanApplication> GetById(LoanApplicationId loanApplicationId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new LoanApplicationEventStreamId(loanApplicationId));

            if (domainEvents.Count == 0)
                return null;

            return new LoanApplication(loanApplicationId, domainEvents.Cast<LoanApplicationDomainEvent>());
        }

        public async Task Save(LoanApplication application)
        {
            foreach (var uncommittedEvent in application.UncommittedEvents)
            {
                uncommittedEvent.AggregateId = application.Id;
                uncommittedEvent.Occurred = DateTimeOffset.Now;
            }
            
            await _eventStore.SaveEvents(new LoanApplicationEventStreamId(application.Id), application.GetExpectedVersion(), application.UncommittedEvents);
        }
    }
}