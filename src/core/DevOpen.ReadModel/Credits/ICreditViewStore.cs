using System.Collections.Generic;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.ReadModel.Credits
{
    public interface ICreditViewStore
    {
        Task<CreditViewModel> GetById(CreditId creditId);

        Task Upsert(CreditViewModel viewModel);

        Task<IEnumerable<CreditViewModel>> GetAllForCountry(Country country);
        Task<IEnumerable<CreditViewModel>> GetAllForOrganisationNumber(OrganisationNumber organisationNumber);
    }
}