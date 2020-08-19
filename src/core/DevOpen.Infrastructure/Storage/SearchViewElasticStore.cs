using System.Threading.Tasks;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.LoanApplications.Model;
using DevOpen.ReadModel.Search;

namespace DevOpen.Infrastructure.Storage
{
    public class SearchViewElasticStore : ElasticViewStore, ISearchViewStore
    {
        public async Task<SearchResult> SearchForTerm(string searchInput)
        {
            var cleanedSearchInput = searchInput.Trim().ToLowerInvariant();
            var searchResponse = await Client.SearchAsync<SearchableElasticView>(
                descriptor => descriptor.AllIndices().From(0).Size(1000)
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
    }
}