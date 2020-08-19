using System.Threading.Tasks;

namespace DevOpen.ReadModel.Search
{
    public interface ISearchViewStore
    {
        Task<SearchResult> SearchForTerm(string searchInput);
    }
}