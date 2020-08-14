using System.Threading.Tasks;
using DevOpen.Domain.Model.Credits;

namespace DevOpen.Application.Repositories
{
    public interface ICreditTransactionRepository
    {
        Task<CreditTransactions> GetById(CreditId id);

        Task Save(CreditTransactions transactions);
    }
}