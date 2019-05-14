using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class DenyCreditApplication : Command
    {
        public CreditApplicationId Id { get; }
        
        public DenyCreditApplication(CreditApplicationId applicationId)
        {
            Id = applicationId;
        }
    }
}