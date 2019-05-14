using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model;

namespace DevOpen.Application.Repositories
{
    public interface ILoanApplicationRepository
    {
        Task<LoanApplication> GetById(LoanApplicationId applicationId);

        Task Save(LoanApplication application);
    }
}