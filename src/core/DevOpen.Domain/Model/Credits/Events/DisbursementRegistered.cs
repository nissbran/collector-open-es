using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.Credits.Events
{
    [EventType("DisbursementRegistered")]
    public class DisbursementRegistered : CreditDomainEvent
    {
        public DisbursementId DisbursementId { get; }
        public Money Amount { get; }

        public DisbursementRegistered(DisbursementId disbursementId, Money amount)
        {
            DisbursementId = disbursementId;
            Amount = amount;
        }
    }
}