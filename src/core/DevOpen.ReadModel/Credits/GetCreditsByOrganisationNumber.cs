using System.Collections.Generic;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;

namespace DevOpen.ReadModel.Credits
{
    public class GetCreditsByOrganisationNumber : Query<IList<CreditId>>
    {
        public OrganisationNumber OrganisationNumber { get; }

        public GetCreditsByOrganisationNumber(OrganisationNumber organisationNumber)
        {
            OrganisationNumber = organisationNumber;
        }
    }
}