using System.Collections.Generic;
using System.Threading.Tasks;
using DevOpen.Domain.Model.Credits;
using DevOpen.ReadModel.Credits;

namespace DevOpen.Application.Handlers.Queries.Credits
{
    public class GetCreditsByOrganisationNumberHandler : QueryHandler<GetCreditsByOrganisationNumber, IList<CreditId>>
    {
        private readonly ICreditLookup _creditLookup;

        public GetCreditsByOrganisationNumberHandler(ICreditLookup creditLookup)
        {
            _creditLookup = creditLookup;
        }
        
        public override async Task<IList<CreditId>> Handle(GetCreditsByOrganisationNumber query)
        {
            return await _creditLookup.GetCreditsForOrganisationNumber(query.OrganisationNumber);
        }
    }
}