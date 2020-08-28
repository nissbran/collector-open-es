using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;

namespace DevOpen.Infrastructure.Persistence.InMemory
{
    public class CreditInMemoryViewStore : ICreditViewStore
    {
        public Dictionary<CreditId, CreditViewModel> Credits { get; } = new Dictionary<CreditId, CreditViewModel>();
        
        public Task<CreditViewModel> GetById(CreditId creditId)
        {
            return Task.FromResult(Credits.ContainsKey(creditId) ? Credits[creditId] : null);
        }

        public Task Upsert(CreditViewModel viewModel)
        {
            if (Credits.ContainsKey(viewModel.Id))
                Credits[viewModel.Id] = viewModel;
            else
                Credits.Add(viewModel.Id, viewModel);
            
            return Task.CompletedTask;
        }

        public Task<IEnumerable<CreditViewModel>> GetAllForCountry(Country country)
        {
            return Task.FromResult(Credits.Values.Where(model => model.Country == country));
        }

        public Task<IEnumerable<CreditViewModel>> GetAllForOrganisationNumber(OrganisationNumber organisationNumber)
        {
            return Task.FromResult(Credits.Values.Where(model => model.OrganisationNumber == organisationNumber));
        }

        public void ClearAll()
        {
            Credits.Clear();
        }
    }
}