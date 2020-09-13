using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Handlers;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.Application.Handlers.Queries.Credits
{
    public class GetCreditsByCountryHandler : QueryHandler<GetCreditsByCountry, List<CreditViewModel>>
    {
        private readonly ICreditViewStore _viewStore;

        public GetCreditsByCountryHandler(ICreditViewStore viewStore)
        {
            _viewStore = viewStore;
        }
        
        public override async Task<List<CreditViewModel>> Handle(GetCreditsByCountry query)
        {
            return (await _viewStore.GetAllForCountry(query.Country)).ToList();
        }
    }
}