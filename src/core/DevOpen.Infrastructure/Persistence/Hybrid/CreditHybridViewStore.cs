using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.Infrastructure.Persistence.InMemory;
using DevOpen.Infrastructure.Storage;
using DevOpen.ReadModel;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;
using Serilog;

namespace DevOpen.Infrastructure.Persistence.Hybrid
{
    public class CreditHybridViewStore : ICreditViewStore, ISwitchable
    {
        private readonly CreditInMemoryViewStore _inMemoryViewStore;
        private readonly CreditViewElasticStore _elasticViewStore;
        private ICreditViewStore _current;

        public CreditHybridViewStore(CreditInMemoryViewStore inMemoryViewStore, CreditViewElasticStore elasticViewStore)
        {
            _inMemoryViewStore = inMemoryViewStore;
            _elasticViewStore = elasticViewStore;
            _current = inMemoryViewStore;
        }

        public async Task<CreditViewModel> GetById(CreditId creditId) => await _current.GetById(creditId);

        public async Task Upsert(CreditViewModel viewModel) => await _current.Upsert(viewModel);

        public async Task<IEnumerable<CreditViewModel>> GetAllForCountry(Country country) => await _current.GetAllForCountry(country);

        public async Task<IEnumerable<CreditViewModel>> GetAllForOrganisationNumber(OrganisationNumber organisationNumber)
        {
            return await _current.GetAllForOrganisationNumber(organisationNumber);
        }

        public void ClearAll()
        {
            _inMemoryViewStore.ClearAll();
            _elasticViewStore.ClearAll();
        }

        public void Switch()
        {
            if (_current != _inMemoryViewStore) return;
            
            var sw = Stopwatch.StartNew();
            _elasticViewStore.Transfer(_inMemoryViewStore.Credits.Values);
            Log.Information("Transferred {Count} credit views to Elastic storage! Elapsed: {Elapsed}", _inMemoryViewStore.Credits.Count, sw.Elapsed);
            _current = _elasticViewStore;
            _inMemoryViewStore.ClearAll();
        }
    }
}