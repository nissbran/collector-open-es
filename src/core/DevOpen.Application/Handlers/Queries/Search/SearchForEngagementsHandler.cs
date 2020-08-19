using System.Threading.Tasks;
using DevOpen.ReadModel.Search;

namespace DevOpen.Application.Handlers.Queries.Search
{
    public class SearchForEngagementsHandler : QueryHandler<SearchForEngagements, SearchResult>
    {
        private readonly ISearchViewStore _searchViewStore;

        public SearchForEngagementsHandler(ISearchViewStore searchViewStore)
        {
            _searchViewStore = searchViewStore;
        }
        
        public override async Task<SearchResult> Handle(SearchForEngagements query)
        {
            return await _searchViewStore.SearchForTerm(query.SearchTerm);
        }
    }
}