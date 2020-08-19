using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.ReadModel.Credits.Model
{
    public class CreditViewModel
    {
        public CreditId Id { get; }
        
        public CreditViewModel(CreditId id)
        {
            Id = id;
        }
        
        public LoanApplicationId ApplicationId { get; set; } = LoanApplicationId.Empty;
        
        public Money LoanAmount { get; set; }
        
        public OrganisationNumber OrganisationNumber { get; set; }
        
        public Money Balance { get; set; }
        public long CreditNumber { get; set; }
    }
}