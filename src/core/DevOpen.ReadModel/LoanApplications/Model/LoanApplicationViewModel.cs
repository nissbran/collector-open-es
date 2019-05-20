using DevOpen.Domain.Model;
using DevOpen.Domain.Model.LoanApplications;

namespace DevOpen.ReadModel.LoanApplications.Model
{
    public class LoanApplicationViewModel
    {
        public LoanApplicationId Id { get; }
        
        public LoanApplicationViewModel(LoanApplicationId id)
        {
            Id = id;
        }
        
        public Money RequestedAmount { get; internal set; }
        
        public OrganisationNumber OrganisationNumber { get; internal set;}
        
        public LoanApplicationStatus Status { get; internal set; }
    }
}