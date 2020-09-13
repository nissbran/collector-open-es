using System.Threading.Tasks;
using DevOpen.Domain.Model.Credits;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.ReadModel.Credits
{
    public interface ICreditLiveProjectionRepository
    {
        Task<CreditViewModel> GetById(CreditId creditId);
    }
}