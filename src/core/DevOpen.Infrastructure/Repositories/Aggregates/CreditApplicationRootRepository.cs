using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Repositories.Aggregates;
using DevOpen.Domain;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories
{
    public class CreditApplicationRootRepository : ICreditApplicationRepository
    {
        private readonly IEventStore _eventStore;

        public CreditApplicationRootRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<CreditApplication> GetById(CreditApplicationId creditApplicationId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new CreditApplicationEventStreamId(creditApplicationId));

            if (domainEvents.Count == 0)
                return null;

            return new CreditApplication(creditApplicationId, domainEvents.Cast<CreditApplicationDomainEvent>());
        }

        public async Task Save(CreditApplication application)
        {
            foreach (var uncommittedEvent in application.UncommittedEvents)
            {
                uncommittedEvent.AggregateId = application.Id;
            }
            
            await _eventStore.SaveEvents(new CreditApplicationEventStreamId(application.Id), application.GetExpectedVersion(), application.UncommittedEvents);
        }
    }
    
    public sealed class CreditApplicationEventStreamId : EventStreamId
    {
        private readonly string _id;

        public override string StreamName => $"CreditApplication-{_id}";

        public CreditApplicationEventStreamId(CreditApplicationId id)
        {
            _id = id.ToString();
        }
    }
}