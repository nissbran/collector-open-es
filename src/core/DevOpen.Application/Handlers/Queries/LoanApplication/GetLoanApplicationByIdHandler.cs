using System.Threading.Tasks;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.Application.Handlers.Queries.LoanApplication
{
    public class GetLoanApplicationByIdHandler : QueryHandler<GetLoanApplicationById, LoanApplicationViewModel>
    {
        private readonly ILoanApplicationViewRepository _viewRepository;

        public GetLoanApplicationByIdHandler(ILoanApplicationViewRepository viewRepository)
        {
            _viewRepository = viewRepository;
        }
        
        public override async Task<LoanApplicationViewModel> Handle(GetLoanApplicationById query)
        {
            return await _viewRepository.GetById(query.ApplicationId);
        }
    }
}