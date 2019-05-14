using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model;

namespace DevOpen.Application.Repositories.Aggregates
{
    public interface ICreditApplicationRepository
    {
        Task<CreditApplication> GetById(CreditApplicationId applicationId);

        Task Save(CreditApplication application);
    }
}