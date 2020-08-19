using System.Collections.Generic;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.ReadModel.Search
{
    public class SearchResult
    {
        public string SearchTerm { get; }
        public int TotalEngagements => Credits.Count + LoanApplications.Count;
        public List<CreditViewModel> Credits { get; } = new List<CreditViewModel>();
        public List<LoanApplicationViewModel> LoanApplications { get; } = new List<LoanApplicationViewModel>();
        
        public SearchResult(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}