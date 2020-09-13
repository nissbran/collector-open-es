using System;
using System.Collections.Generic;
using DevOpen.Domain.Model.Credits.Commands;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Domain.Model.Credits.Projections;
using DevOpen.Framework.Domain;

namespace DevOpen.Domain.Model.Credits
{
    public class Credit : AggregateRoot<CreditId, CreditState>
    {
        public CreditId Id => State.Id;

        public Credit(CreditId id, IEnumerable<CreditDomainEvent> historicEvents) : base(new CreditState(id, historicEvents))
        {
        }
        
        public Credit(CreditId id, RegisterCredit cmd, long creditNumber) : base(new CreditState(id))
        {
            ApplyChange(new CreditRegistered(cmd.OrganisationNumber, cmd.LoanAmount, creditNumber));
            
            if (!cmd.InvoiceAddress.IsEmpty)
                ApplyChange(new InvoiceAddressRegistered(cmd.InvoiceAddress));
            
            if (cmd.ApplicationId.HasValue)
                ApplyChange(new CreditRegisteredFromLoanApplication(cmd.ApplicationId.Value));
        }

        public void RegisterDisbursementPayout(Money amount)
        {
            if (Math.Abs(State.Balance - amount) <= State.LoanAmount)
                ApplyChange(new DisbursementRegistered(DisbursementId.NewId(), amount));
        }
    }
}