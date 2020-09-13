using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Framework.Infrastructure.Persistence.EventStoreDb;
using DevOpen.Infrastructure.Repositories.Aggregates;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;
using DevOpen.ReadModel.LoanApplications.Projections;

namespace DevOpen.Infrastructure.Repositories.Views
{
    public class ApplicationLiveProjectionRepository : IApplicationLiveProjectionRepository
    {
        private readonly IEventStore _eventStore;
        private readonly LoanApplicationViewProjection _projection;

        public ApplicationLiveProjectionRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
            _projection = new LoanApplicationViewProjection();
        }
        
        public async Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId)
        {
            var domainEvents = await _eventStore.GetEventsByStreamId(new LoanApplicationEventStreamId(applicationId));

            if (domainEvents.Count == 0)
                return null;

            var model = new LoanApplicationViewModel(applicationId);
            
            foreach (var domainEvent in domainEvents.Cast<LoanApplicationDomainEvent>())
            {
                _projection.Apply(model, domainEvent);
            }
            
            return model;
        }
    }
}