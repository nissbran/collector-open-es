using System;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Framework.Infrastructure.Persistence.EventStoreDb;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public class LoanApplicationAggregateStore : ILoanApplicationAggregateStore
    {
        private readonly IEventStore _eventStore;

        public LoanApplicationAggregateStore(IEventStore eventStore)
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