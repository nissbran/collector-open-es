namespace DevOpen.Domain.Model.Credits.Commands
{
    public class RegisterDisbursementPayout : Command
    {
        public CreditId Id { get; }
        
        public Money Amount { get; }
        
        public RegisterDisbursementPayout(CreditId id, Money amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}