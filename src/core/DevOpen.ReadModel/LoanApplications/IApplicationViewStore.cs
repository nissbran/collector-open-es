using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.ReadModel.LoanApplications
{
    public interface IApplicationViewStore
    {
        Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId);

        Task Upsert(LoanApplicationViewModel viewModel);

        void ClearAll();
    }
}