using System.Threading.Tasks;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.Application.Handlers.Queries.Credits
{
    public class GetCreditByIdHandler : QueryHandler<GetCreditById, CreditViewModel>
    {
        private readonly ICreditViewRepository _viewRepository;

        public GetCreditByIdHandler(ICreditViewRepository viewRepository)
        {
            _viewRepository = viewRepository;
        }
        
        public override async Task<CreditViewModel> Handle(GetCreditById query)
        {
            return await _viewRepository.GetById(query.CreditId);
        }
    }
}