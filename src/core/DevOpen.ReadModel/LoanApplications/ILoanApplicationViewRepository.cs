using System.Threading.Tasks;
using DevOpen.Domain.Model;

namespace DevOpen.ReadModel.LoanApplications
{
    public interface ILoanApplicationViewRepository
    {
        Task<LoanApplicationView> GetById(LoanApplicationId applicationId);
    }
}