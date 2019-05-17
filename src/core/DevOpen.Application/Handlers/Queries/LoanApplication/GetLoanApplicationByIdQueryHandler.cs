using System.Threading.Tasks;
using DevOpen.ReadModel.LoanApplications;

namespace DevOpen.Application.Handlers.Queries.LoanApplication
{
    public class GetLoanApplicationByIdQueryHandler : QueryHandler<GetLoanApplicationByIdQuery, LoanApplicationViewModel>
    {
        private readonly ILoanApplicationViewRepository _viewRepository;

        public GetLoanApplicationByIdQueryHandler(ILoanApplicationViewRepository viewRepository)
        {
            _viewRepository = viewRepository;
        }
        
        public override async Task<LoanApplicationViewModel> Handle(GetLoanApplicationByIdQuery query)
        {
            return await _viewRepository.GetById(query.ApplicationId);
        }
    }
}