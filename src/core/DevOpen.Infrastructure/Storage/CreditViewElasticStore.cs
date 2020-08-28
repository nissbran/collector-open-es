using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.Infrastructure.Serialization;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;
using Nest;

namespace DevOpen.Infrastructure.Storage
{
    public class CreditViewElasticStore : ICreditViewStore
    {
        private readonly ElasticConnectionProvider _connectionProvider;
        private readonly IndexName _index;
        private readonly ViewModelSerializer _serializer;

        public CreditViewElasticStore(ElasticConnectionProvider connectionProvider, ElasticIndexNameResolver indexNameResolver)
        {
            _connectionProvider = connectionProvider;
            _index = indexNameResolver.ResolveIndexName<CreditViewModel>();
            _serializer = new ViewModelSerializer();
        }
        
        public async Task<CreditViewModel> GetById(CreditId creditId)
        {
            var response = await _connectionProvider.Client.GetAsync<SearchableElasticView>(
                new GetRequest<SearchableElasticView>(_index, creditId.ToString()));

            return response.Found ? _serializer.Deserialize<CreditViewModel>(response.Source.JsonData) : null;
        }

        public async Task Upsert(CreditViewModel viewModel)
        {
            await _connectionProvider.Client.IndexAsync(
                new IndexRequest<SearchableElasticView>(CreateElasticView(viewModel), _index, viewModel.Id.ToString()));
        }

        public async Task<IEnumerable<CreditViewModel>> GetAllForCountry(Country country)
        {
            var searchResponse = await _connectionProvider.Client.SearchAsync<SearchableElasticView>(
                descriptor => descriptor.Index(_index).From(0).Size(1000)
                    .Query(query => query.Term(view => view.CountryCode, country.CodeSymbol.ToLowerInvariant())));
                    
            if (!searchResponse.IsValid)
                return new List<CreditViewModel>();

            return searchResponse.Documents.Select(view => _serializer.Deserialize<CreditViewModel>(view.JsonData));
        }

        public async Task<IEnumerable<CreditViewModel>> GetAllForOrganisationNumber(OrganisationNumber organisationNumber)
        {
            var searchResponse = await _connectionProvider.Client.SearchAsync<SearchableElasticView>(
                descriptor => descriptor.Index(_index).From(0).Size(100)
                    .Query(query => query.Term(view => view.OrganisationNumber, organisationNumber.ToString().ToLowerInvariant())));
            
            if (!searchResponse.IsValid)
                return new List<CreditViewModel>();

            return searchResponse.Documents.Select(view => _serializer.Deserialize<CreditViewModel>(view.JsonData));
        }

        public void ClearAll()
        {
            _connectionProvider.Client.Indices.Delete(_index);
        }

        public void Transfer(IEnumerable<CreditViewModel> viewModels)
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

        private SearchableElasticView CreateElasticView(CreditViewModel viewModel)
        {
            var elasticView = new SearchableElasticView()
            {
                Id = viewModel.Id, 
                ViewType = nameof(CreditViewModel),
                JsonData = _serializer.Serialize(viewModel),
                CreditNumber = viewModel.CreditNumber.ToString(),
                Currency = viewModel.LoanAmount.Currency.Code,
                CountryCode = viewModel.OrganisationNumber.Country.CodeSymbol,
                OrganisationNumber = viewModel.OrganisationNumber.ToString()
            };
            return elasticView;
        }
    }
}