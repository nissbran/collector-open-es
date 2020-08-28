using System.Threading.Tasks;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.Infrastructure.Serialization;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.LoanApplications.Model;
using DevOpen.ReadModel.Search;
using Nest;

namespace DevOpen.Infrastructure.Storage
{
    public class SearchViewElasticStore : ISearchViewStore
    {
        private readonly ElasticConnectionProvider _connectionProvider;
        private readonly ElasticIndexNameResolver _indexNameResolver;
        private readonly ViewModelSerializer _serializer;

        public SearchViewElasticStore(ElasticConnectionProvider connectionProvider, ElasticIndexNameResolver indexNameResolver)
        {
            _connectionProvider = connectionProvider;
            _indexNameResolver = indexNameResolver;
            _serializer = new ViewModelSerializer();
        }
        
        public async Task<SearchResult> SearchForTerm(string searchInput)
        {
            var cleanedSearchInput = searchInput.Trim().ToLowerInvariant();
            var searchResponse = await _connectionProvider.Client.SearchAsync<SearchableElasticView>(
                descriptor => descriptor.Index(IndicesToSearch()).From(0).Size(1000)
                    .Query(query => query.Term(view => view.CountryCode, cleanedSearchInput) ||
                                    query.Wildcard(m => m
                                        .Field(view => view.CreditNumber)
                                        .Value(cleanedSearchInput + "*")
                                    )));

            var searchResult = new SearchResult(searchInput);
            if (!searchResponse.IsValid)
                return searchResult;

            foreach (var elasticView in searchResponse.Documents)
            {
                switch (elasticView.ViewType)
                {
                    case nameof(CreditViewModel):
                        searchResult.Credits.Add(ParseViewModel<CreditViewModel>(elasticView));
                        break;
                    case nameof(LoanApplicationViewModel):
                        searchResult.LoanApplications.Add(ParseViewModel<LoanApplicationViewModel>(elasticView));
                        break;
                }
            }
            
            return searchResult;
        }
        
        private T ParseViewModel<T>(ElasticView elasticView) where T : class
        {
            return _serializer.Deserialize<T>(elasticView.JsonData);
        }

        private Indices IndicesToSearch()
        {
            return Indices.Index(
                _indexNameResolver.ResolveIndexName<CreditViewModel>(),
                _indexNameResolver.ResolveIndexName<LoanApplicationViewModel>());
        }
    }
}