using DevOpen.Domain.Model.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;

namespace DevOpen.ReadModel.LoanApplications
{
    public class GetLoanApplicationById : Query<LoanApplicationViewModel>
    {
        public LoanApplicationId ApplicationId { get; }

        public GetLoanApplicationById(LoanApplicationId applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}