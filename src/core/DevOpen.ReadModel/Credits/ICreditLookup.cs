using System.Collections.Generic;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;

namespace DevOpen.ReadModel.Credits
{
    public interface ICreditLookup
    {
        Task<IList<CreditId>> GetCreditsForOrganisationNumber(OrganisationNumber organisationNumber);
        Task AddCreditToLookup(CreditId creditId, OrganisationNumber organisationNumber);
    }
}