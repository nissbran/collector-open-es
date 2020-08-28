using System.Collections.Generic;
using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.Infrastructure.Persistence.InMemory
{
    public class ApplicationInMemoryViewStore : IApplicationViewStore
    {
        public Dictionary<LoanApplicationId, LoanApplicationViewModel> Applications { get; } = new Dictionary<LoanApplicationId, LoanApplicationViewModel>();
        
        public Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId)
        {
            return Task.FromResult(Applications.ContainsKey(applicationId) ? Applications[applicationId] : null);
        }

        public Task Upsert(LoanApplicationViewModel viewModel)
        {
            if (Applications.ContainsKey(viewModel.Id))
                Applications[viewModel.Id] = viewModel;
            else
                Applications.Add(viewModel.Id, viewModel);
            
            return Task.CompletedTask;
        }

        public void ClearAll()
        {
            Applications.Clear();
        }
    }
}