using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.Infrastructure.Serialization;
using DevOpen.ReadModel;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;
using Nest;

namespace DevOpen.Infrastructure.Storage
{
    public class ApplicationViewElasticStore : IApplicationViewStore, ISwitchable
    {
        private readonly ElasticConnectionProvider _connectionProvider;
        private readonly IndexName _index;
        private readonly ViewModelSerializer _serializer;

        public ApplicationViewElasticStore(
            ElasticConnectionProvider connectionProvider,
            ElasticIndexNameResolver indexNameResolver)
        {
            _connectionProvider = connectionProvider;
            _index = indexNameResolver.ResolveIndexName<LoanApplicationViewModel>();
            _serializer = new ViewModelSerializer();
        }
        
        public async Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId)
        {
            var response = await _connectionProvider.Client.GetAsync<SearchableElasticView>(
                new GetRequest<SearchableElasticView>(_index, applicationId.ToString()));

            return response.Found ? _serializer.Deserialize<LoanApplicationViewModel>(response.Source.JsonData) : null;
        }

        public async Task Upsert(LoanApplicationViewModel viewModel)
        {
            await _connectionProvider.Client.IndexAsync(
                new IndexRequest<SearchableElasticView>(CreateElasticView(viewModel), _index, viewModel.Id.ToString()));
        }

        public void ClearAll()
        {
            _connectionProvider.Client.Indices.Delete(_index);
        }

        public void Transfer(IEnumerable<LoanApplicationViewModel> viewModels)
        {
            var bulkAllObservable = _connectionProvider.Client.BulkAll(viewModels.Select(CreateElasticView), b => b
                    .Index(_index)
                    .BackOffTime("30s") 
                    .BackOffRetries(2) 
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000) 
                )
                .Wait(TimeSpan.FromMinutes(15), next => 
                {
                    // do something e.g. write number of pages to console
                });
        }

        private SearchableElasticView CreateElasticView(LoanApplicationViewModel viewModel)
        {
            var elasticView = new SearchableElasticView
            {
                Id = viewModel.Id, 
                ViewType = nameof(LoanApplicationViewModel),
                JsonData = _serializer.Serialize(viewModel),
                CreditNumber = viewModel.CreditNumber.ToString(),
                Currency = viewModel.RequestedAmount.Currency.Code,
                CountryCode = viewModel.OrganisationNumber.Country.CodeSymbol,
                OrganisationNumber = viewModel.OrganisationNumber.ToString()
            };
            return elasticView;
        }

        public void Switch()
        {
            
        }
    }
}