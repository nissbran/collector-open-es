namespace DevOpen.ReadModel.Search
{
    public class SearchForEngagements : Query<SearchResult>
    {
        public string SearchTerm { get; }
        
        public SearchForEngagements(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}