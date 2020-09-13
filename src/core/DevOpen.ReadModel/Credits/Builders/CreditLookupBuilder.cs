using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.Framework.Domain;
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
                    await _creditLookup.AddCreditToLookup(creditRegistered.Id, creditRegistered.OrganisationNumber);
                    break;
            }
        }

        public void ClearModel()
        {   
            _creditLookup.ClearAll();
        }

        public void Switch()
        {
        }
    }
}