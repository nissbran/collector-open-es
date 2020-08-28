using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.ReadModel.LoanApplications.Model;
using DevOpen.ReadModel.LoanApplications.Projections;

namespace DevOpen.ReadModel.LoanApplications.Builders
{
    public class ApplicationViewBuilder : IReadModelBuilder
    {
        private readonly IApplicationViewStore _applicationViewStore;
        private readonly LoanApplicationViewProjection _projection;

        public ApplicationViewBuilder(IApplicationViewStore applicationViewStore)
        {
            _applicationViewStore = applicationViewStore;
            _projection = new LoanApplicationViewProjection();
        }
        
        public async Task Handle(DomainEvent domainEvent)
        {
            if (domainEvent is LoanApplicationDomainEvent applicationDomainEvent)
            {
                var model = await _applicationViewStore.GetById(applicationDomainEvent.ApplicationId) ?? new LoanApplicationViewModel(applicationDomainEvent.ApplicationId);

                _projection.Apply(model, applicationDomainEvent);

                await _applicationViewStore.Upsert(model);
            }
        }

        public void ClearModel()
        {
            _applicationViewStore.ClearAll();
        }

        public void Switch()
        {
            if (_applicationViewStore is ISwitchable switchableStore)
            {
                switchableStore.Switch();
            }
        }
    }
}