using System.Threading.Tasks;

namespace DevOpen.Application.Repositories
{
    public interface ICreditNumberRepository
    {
        Task<long> GetCurrentAsync();
        Task<long> GetNextAsync();
    }
}