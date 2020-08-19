using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model.Credits;
using DevOpen.ReadModel.Credits;

namespace DevOpen.Application.Handlers.Queries.Credits
{
    public class GetCreditsByOrganisationNumberHandler : QueryHandler<GetCreditsByOrganisationNumber, IList<CreditId>>
    {
        private readonly ICreditViewStore _viewStore;

        public GetCreditsByOrganisationNumberHandler(ICreditViewStore viewStore)
        {
            _viewStore = viewStore;
        }
        
        public override async Task<IList<CreditId>> Handle(GetCreditsByOrganisationNumber query)
        {
            return (await _viewStore.GetAllForOrganisationNumber(query.OrganisationNumber)).Select(model => model.Id).ToList();
        }
    }
}