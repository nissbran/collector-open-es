using System.Threading.Tasks;
using DevOpen.Framework.Application.Handlers;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.Application.Handlers.Queries.LoanApplication
{
    public class GetLoanApplicationByIdHandler : QueryHandler<GetLoanApplicationById, LoanApplicationViewModel>
    {
        private readonly IApplicationLiveProjectionRepository _liveProjectionRepository;

        public GetLoanApplicationByIdHandler(IApplicationLiveProjectionRepository liveProjectionRepository)
        {
            _liveProjectionRepository = liveProjectionRepository;
        }
        
        public override async Task<LoanApplicationViewModel> Handle(GetLoanApplicationById query)
        {
            return await _liveProjectionRepository.GetById(query.ApplicationId);
        }
    }
}