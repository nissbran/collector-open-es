using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.ReadModel.LoanApplications
{
    public interface ILoanApplicationViewRepository
    {
        Task<LoanApplicationView> GetById(LoanApplicationId applicationId);
    }
}