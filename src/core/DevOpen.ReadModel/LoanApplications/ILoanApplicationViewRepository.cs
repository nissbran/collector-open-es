using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.ReadModel.LoanApplications
{
    public interface ILoanApplicationViewRepository
    {
        Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId);
    }
}