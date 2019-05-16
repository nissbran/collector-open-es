using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Repositories.Aggregates;
using DevOpen.ReadModel.LoanApplications;

namespace DevOpen.Infrastructure.Repositories.Views
{
    public class LoanApplicationViewRepository : ILoanApplicationViewRepository
    {
        private readonly IEventStore _eventStore;

        public LoanApplicationViewRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }
        
        public async Task<LoanApplicationView> GetById(LoanApplicationId applicationId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new LoanApplicationEventStreamId(applicationId));

            if (domainEvents.Count == 0)
                return null;
            
            return new LoanApplicationView(domainEvents.Cast<LoanApplicationDomainEvent>());
        }
    }
}