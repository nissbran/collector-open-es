using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class ApproveCreditApplication : Command
    {
        public CreditApplicationId Id { get; }
        
        public ApproveCreditApplication(CreditApplicationId applicationId)
        {
            Id = applicationId;
        }
    }
}