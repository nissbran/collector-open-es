using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model.Credits.Events;
using Serilog;

namespace DevOpen.ReadModel.Credits.Builders
{
    public class CreditLookupBuilder : IReadModelBuilder
    {
        private readonly ICreditLookup _creditLookup;

        public CreditLookupBuilder(ICreditLookup creditLookup)
        {
            _creditLookup = creditLookup;
        }
        
        public async Task Handle(DomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case CreditRegistered creditRegistered:
                    Log.Information("Handling {Event}, adding credit to lookup read model", nameof(CreditRegistered));
                    
                    await _creditLookup.AddCreditToLookup(creditRegistered.Id, creditRegistered.OrganisationNumber);
                    
                    break;
            }
        }
    }
}