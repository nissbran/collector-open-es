using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.ReadModel.LoanApplications
{
    public interface ILoanApplicationViewRepository
    {
        Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId);
    }
}