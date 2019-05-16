using System;

namespace DevOpen.Domain.Model.Credits
{
    internal class Disbursement
    {
        public DisbursementId Id { get; }
        
        public Money Amount { get; }
        
        public bool PayoutRegistered { get; private set; }
        
        public DateTimeOffset? RegistrationDate { get; private set; }

        public Disbursement(DisbursementId id, Money amount)
        {
            Id = id;
            Amount = amount;
        }

        public void Register(DateTimeOffset registrationDate)
        {
            PayoutRegistered = true;
            RegistrationDate = registrationDate;
        }
    }
}