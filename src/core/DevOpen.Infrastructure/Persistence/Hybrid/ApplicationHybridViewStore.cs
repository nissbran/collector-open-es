using System.Diagnostics;
using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.Infrastructure.Persistence.InMemory;
using DevOpen.Infrastructure.Storage;
using DevOpen.ReadModel;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;
using Serilog;

namespace DevOpen.Infrastructure.Persistence.Hybrid
{
    public class ApplicationHybridViewStore : IApplicationViewStore, ISwitchable
    {
        private readonly ApplicationInMemoryViewStore _inMemoryViewStore;
        private readonly ApplicationViewElasticStore _elasticViewStore;
        private IApplicationViewStore _current;

        public ApplicationHybridViewStore(ApplicationInMemoryViewStore inMemoryViewStore, ApplicationViewElasticStore elasticViewStore)
        {
            _inMemoryViewStore = inMemoryViewStore;
            _elasticViewStore = elasticViewStore;
            _current = inMemoryViewStore;
        }
        
        public async Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId)
        {
            return await _current.GetById(applicationId);
        }

        public async Task Upsert(LoanApplicationViewModel viewModel)
        {
            await _current.Upsert(viewModel);
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
            _elasticViewStore.Transfer(_inMemoryViewStore.Applications.Values);
            Log.Information("Transferred {Count} application views to Elastic storage! Elapsed: {Elapsed}", _inMemoryViewStore.Applications.Count, sw.Elapsed);
            _current = _elasticViewStore;
            _inMemoryViewStore.ClearAll();
        }
    }
}