using System;
using DevOpen.Domain.Model;

namespace DevOpen.Domain.Events
{
    [EventType("DisbursementPayoutRegistered")]
    public class DisbursementPayoutRegistered : CreditDomainEvent
    {
        public DisbursementId DisbursementId { get; }
        public DateTimeOffset RegistrationDate { get; }

        public DisbursementPayoutRegistered(DisbursementId disbursementId, DateTimeOffset registrationDate)
        {
            DisbursementId = disbursementId;
            RegistrationDate = registrationDate;
        }
    }
}