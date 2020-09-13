using System.Threading.Tasks;
using DevOpen.Framework.Application.Handlers;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.Application.Handlers.Queries.Credits
{
    public class GetCreditByIdHandler : QueryHandler<GetCreditById, CreditViewModel>
    {
        private readonly ICreditLiveProjectionRepository _liveProjectionRepository;

        public GetCreditByIdHandler(ICreditLiveProjectionRepository liveProjectionRepository)
        {
            _liveProjectionRepository = liveProjectionRepository;
        }
        
        public override async Task<CreditViewModel> Handle(GetCreditById query)
        {
            return await _liveProjectionRepository.GetById(query.CreditId);
        }
    }
}