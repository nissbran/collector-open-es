using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model.Credits.Events;

namespace DevOpen.ReadModel.Credits
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
                    await _creditLookup.AddCreditToLookup(creditRegistered.Id, creditRegistered.OrganisationNumber);
                    break;
            }
        }
    }
}