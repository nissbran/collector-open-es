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
        
        public LoanApplicationId ApplicationId { get; internal set; } = LoanApplicationId.Empty;
        
        public Money LoanAmount { get; internal set; }
        
        public OrganisationNumber OrganisationNumber { get; internal set; }
        
        public Money Balance { get; internal set; }
    }
}