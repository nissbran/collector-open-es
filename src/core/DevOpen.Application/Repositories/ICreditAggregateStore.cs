using System.Threading.Tasks;
using DevOpen.Domain.Model.Credits;

namespace DevOpen.Application.Repositories
{
    public interface ICreditAggregateStore
    {
        Task<Credit> GetById(CreditId id);

        Task Save(Credit credit);
    }
}