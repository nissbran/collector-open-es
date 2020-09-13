using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Framework.Application.ReadModels;

namespace DevOpen.ReadModel.Credits.Model
{
    public class CreditViewModel : IViewModel
    {
        public CreditId Id { get; }
        
        public CreditViewModel(CreditId id)
        {
            Id = id;
        }
        
        public LoanApplicationId ApplicationId { get; set; } = LoanApplicationId.Empty;
        public Money LoanAmount { get; set; }
        public OrganisationNumber OrganisationNumber { get; set; }
        public Country Country { get; set; }
        public Money Balance { get; set; }
        public long CreditNumber { get; set; }
    }
}