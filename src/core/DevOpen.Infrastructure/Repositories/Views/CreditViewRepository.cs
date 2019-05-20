using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Repositories.Aggregates;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.Credits.Projections;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;
using DevOpen.ReadModel.LoanApplications.Projections;

namespace DevOpen.Infrastructure.Repositories.Views
{
    public class CreditViewRepository : ICreditViewRepository
    {
        private readonly IEventStore _eventStore;
        private readonly CreditViewProjection _projection;

        public CreditViewRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
            _projection = new CreditViewProjection();
        }
        
        public async Task<CreditViewModel> GetById(CreditId creditId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new CreditEventStreamId(creditId));

            if (domainEvents.Count == 0)
                return null;

            var model = new CreditViewModel(creditId);
            
            foreach (var domainEvent in domainEvents.Cast<CreditDomainEvent>())
            {
                _projection.Apply(model, domainEvent);
            }
            
            return model;
        }
    }
}