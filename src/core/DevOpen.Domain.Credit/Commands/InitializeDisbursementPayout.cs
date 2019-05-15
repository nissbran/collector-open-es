using DevOpen.Domain.Model;

namespace DevOpen.Domain.Commands
{
    public class InitializeDisbursementPayout : Command
    {
        public CreditId Id { get; }
        
        public Money Amount { get; }
        
        public InitializeDisbursementPayout(CreditId id, Money amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}