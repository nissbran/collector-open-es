using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;

namespace DevOpen.Application.Repositories
{
    public interface ICreditRootRepository
    {
        Task<Credit> GetById(CreditId id);

        Task Save(Credit credit);
    }
}