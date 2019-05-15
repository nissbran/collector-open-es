using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model;

namespace DevOpen.Application.Repositories
{
    public interface ICreditRootRepository
    {
        Task<Credit> GetById(CreditId id);

        Task Save(Credit credit);
    }
}