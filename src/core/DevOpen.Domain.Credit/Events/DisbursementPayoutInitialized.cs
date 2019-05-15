using DevOpen.Domain.Model;

namespace DevOpen.Domain.Events
{
    [EventType("DisbursementPayoutInitialized")]
    public class DisbursementPayoutInitialized : CreditDomainEvent
    {
        public DisbursementId DisbursementId { get; }
        public Money Amount { get; }

        public DisbursementPayoutInitialized(DisbursementId disbursementId, Money amount)
        {
            DisbursementId = disbursementId;
            Amount = amount;
        }
    }
}