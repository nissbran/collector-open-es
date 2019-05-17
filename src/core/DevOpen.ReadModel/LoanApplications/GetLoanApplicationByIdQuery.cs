using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.ReadModel.LoanApplications
{
    public class GetLoanApplicationByIdQuery : Query<LoanApplicationViewModel>
    {
        public LoanApplicationId ApplicationId { get; }

        public GetLoanApplicationByIdQuery(LoanApplicationId applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}