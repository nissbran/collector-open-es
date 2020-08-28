using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.Credits.Projections;

namespace DevOpen.ReadModel.Credits.Builders
{
    public class CreditViewBuilder : IReadModelBuilder
    {
        private readonly ICreditViewStore _creditViewStore;
        private readonly CreditViewProjection _projection;

        public CreditViewBuilder(ICreditViewStore creditViewStore)
        {
            _creditViewStore = creditViewStore;
            _projection = new CreditViewProjection();
        }
        
        public async Task Handle(DomainEvent domainEvent)
        {
            if (domainEvent is CreditDomainEvent creditDomainEvent)
            {
                var model = await _creditViewStore.GetById(creditDomainEvent.Id) ?? new CreditViewModel(creditDomainEvent.Id);

                _projection.Apply(model, creditDomainEvent);

                await _creditViewStore.Upsert(model);
            }
        }

        public void ClearModel()
        {
            _creditViewStore.ClearAll();
        }

        public void Switch()
        {
            if (_creditViewStore is ISwitchable switchableStore)
            {
                switchableStore.Switch();
            }
        }
    }
}