using System;
using System.Collections.Generic;
using DevOpen.Domain.Commands;
using DevOpen.Domain.Events;
using DevOpen.Domain.Model;
using DevOpen.Domain.Projections;

namespace DevOpen.Domain
{
    public class Credit : AggregateRoot<CreditId, CreditState>
    {
        public CreditId Id => State.Id;

        public Credit(CreditId id, IEnumerable<CreditDomainEvent> historicEvents) : base(new CreditState(id, historicEvents))
        {
        }
        
        public Credit(CreditId id, RegisterCredit cmd) : base(new CreditState(id))
        {
            ApplyChange(new CreditRegistered(cmd.OrganisationNumber, cmd.LoanAmount));
            
            if (!cmd.InvoiceAddress.IsEmpty)
                ApplyChange(new InvoiceAddressRegistered(cmd.InvoiceAddress));
            
            if (cmd.ApplicationId.HasValue)
                ApplyChange(new CreditRegisteredFromLoanApplication(cmd.ApplicationId.Value));
        }

        public void InitializeDisbursementPayout(Money amount)
        {
            if (Math.Abs(State.Balance - amount) < State.LoanAmount)
                ApplyChange(new DisbursementPayoutInitialized(DisbursementId.NewId(), amount));
        }

        public void RegisterDisbursementPayout(DisbursementId disbursementId, DateTimeOffset registeredDate)
        {
            ApplyChange(new DisbursementPayoutRegistered(disbursementId, registeredDate));
        }
    }
}