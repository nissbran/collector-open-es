using DevOpen.Domain.Model;

namespace DevOpen.ReadModel.LoanApplications
{
    public class GetLoanApplicationByIdQuery : Query<LoanApplicationView>
    {
        public LoanApplicationId ApplicationId { get; }

        public GetLoanApplicationByIdQuery(LoanApplicationId applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}