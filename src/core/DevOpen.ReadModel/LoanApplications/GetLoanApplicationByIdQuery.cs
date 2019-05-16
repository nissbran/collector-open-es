using DevOpen.Domain.Model;
using DevOpen.Domain.Model.LoanApplications;

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