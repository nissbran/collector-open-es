using System.Linq;
using System.Threading.Tasks;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories
{
//    public class CreditRootRepository : ICreditRootRepository
//    {
//        private readonly IEventStore _eventStore;
//
//        public CreditRootRepository(IEventStore eventStore)
//        {
//            _eventStore = eventStore;
//        }
//
//        public async Task<Credit> GetCreditById(CreditId creditId)
//        {
//            var domainEvents = await _eventStore.GetEventsByStreamId(new CreditEventStreamId(creditId));
//
//            if (domainEvents.Count == 0)
//                return null;
//
//            return new Credit(creditId, domainEvents.Cast<CreditDomainEvent>());
//        }
//
//        public async Task SaveCredit(Credit credit)
//        {
//            foreach (var uncommittedEvent in credit.UncommittedEvents)
//            {
//                uncommittedEvent.AggregateId = credit.Id;
//            }
//            
//            await _eventStore.SaveEvents(new CreditEventStreamId(credit.Id), credit.GetExpectedVersion(), credit.UncommittedEvents);
//        }
//    }
//    
//    public sealed class CreditEventStreamId : EventStreamId
//    {
//        private readonly string _id;
//
//        public override string StreamName => $"Credit-{_id}";
//
//        public CreditEventStreamId(CreditId id)
//        {
//            _id = id.ToString();
//        }
//    }
}