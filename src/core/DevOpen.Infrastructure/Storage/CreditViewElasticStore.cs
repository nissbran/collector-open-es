using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;
using Nest;

namespace DevOpen.Infrastructure.Storage
{
    public class CreditViewElasticStore : ElasticViewStore, ICreditViewStore
    {
        public CreditViewElasticStore() : base(settings => settings.DefaultIndex(ElasticIndices.Credits))
        {
        }
        
        public async Task<CreditViewModel> GetById(CreditId creditId)
        {
            var response = await Client.GetAsync<SearchableElasticView>(creditId.ToString());

            return response.Found ? Serializer.Deserialize<CreditViewModel>(response.Source.JsonData) : null;
        }

        public async Task Upsert(CreditViewModel viewModel)
        {
            var view = CreateElasticView(viewModel);
            await Client.IndexAsync(new IndexRequest<SearchableElasticView>(view, ElasticIndices.Credits, viewModel.Id.ToString()));
        }

        public async Task<IEnumerable<CreditViewModel>> GetAllForCountry(Country country)
        {
            var searchResponse = await Client.SearchAsync<SearchableElasticView>(
                descriptor => descriptor.From(0).Size(1000)
                    .Query(query => query.Term(view => view.CountryCode, country.CodeSymbol.ToLowerInvariant())));
                    
            if (!searchResponse.IsValid)
                return new List<CreditViewModel>();

            return searchResponse.Documents.Select(ParseViewModel<CreditViewModel>);
        }

        public async Task<IEnumerable<CreditViewModel>> GetAllForOrganisationNumber(OrganisationNumber organisationNumber)
        {
            var searchResponse = await Client.SearchAsync<SearchableElasticView>(
                descriptor => descriptor.From(0).Size(100)
                    .Query(query => query.Term(view => view.OrganisationNumber, organisationNumber.ToString().ToLowerInvariant())));
            
            if (!searchResponse.IsValid)
                return new List<CreditViewModel>();

            return searchResponse.Documents.Select(ParseViewModel<CreditViewModel>);
        }

        private SearchableElasticView CreateElasticView(CreditViewModel viewModel)
        {
            var elasticView = new SearchableElasticView()
            {
                Id = viewModel.Id, 
                ViewType = nameof(CreditViewModel),
                JsonData = Serializer.Serialize(viewModel),
                CreditNumber = viewModel.CreditNumber.ToString(),
                Currency = viewModel.LoanAmount.Currency.Code,
                CountryCode = viewModel.OrganisationNumber.Country.CodeSymbol,
                OrganisationNumber = viewModel.OrganisationNumber.ToString()
            };
            return elasticView;
        }
    }
}