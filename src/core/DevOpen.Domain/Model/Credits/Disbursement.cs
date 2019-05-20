using System;

namespace DevOpen.Domain.Model.Credits
{
    internal class Disbursement
    {
        public DisbursementId Id { get; }
        
        public Money Amount { get; }

        public Disbursement(DisbursementId id, Money amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}