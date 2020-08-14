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
        private const string Index = "credits";

        public CreditViewElasticStore() : base(Index)
        {
        }
        
        public async Task<CreditViewModel> GetById(CreditId creditId)
        {
            var response = await Client.GetAsync<CreditElasticView>(creditId.ToString());

            return response.Found ? Serializer.Deserialize<CreditViewModel>(response.Source.JsonData) : null;
        }

        public async Task Upsert(CreditViewModel viewModel)
        {
            var view = CreateElasticView(viewModel);
            await Client.IndexAsync(new IndexRequest<CreditElasticView>(view, Index, view.Id.ToString()));
        }

        public async Task<IEnumerable<CreditViewModel>> GetAllForCountry(Country country)
        {
            var searchResponse = await Client.SearchAsync<CreditElasticView>(
                descriptor => descriptor
                    .Query(query => query.Term(view => view.Parameters.CountryCode, country.CodeSymbol.ToLowerInvariant())));
                    
            if (!searchResponse.IsValid)
                return new List<CreditViewModel>();

            return searchResponse.Documents.Select(ParseViewModel<CreditViewModel>);
        }

        public async Task<IEnumerable<CreditViewModel>> GetAllForOrganisationNumber(OrganisationNumber organisationNumber)
        {
            var searchResponse = await Client.SearchAsync<CreditElasticView>(
                descriptor => descriptor
                    .Query(query => query.Term(view => view.Parameters.OrganisationNumber, organisationNumber.ToString().ToLowerInvariant())));
            if (!searchResponse.IsValid)
                return new List<CreditViewModel>();

            return searchResponse.Documents.Select(ParseViewModel<CreditViewModel>);
        }

        private CreditElasticView CreateElasticView(CreditViewModel viewModel)
        {
            var elasticView = new CreditElasticView
            {
                Id = viewModel.CreditId, 
                JsonData = Serializer.Serialize(viewModel)
            };
            elasticView.UpdateSearchParameters(viewModel);
            return elasticView;
        }
    }
    
    public class CreditElasticView : ElasticView
    {
        public CreditElasticSearchParameters Parameters { get; set; } = new CreditElasticSearchParameters();
        
        public string CountryCode { get; set; }

        public void UpdateSearchParameters(CreditViewModel viewModel)
        {
            CountryCode = viewModel.OrganisationNumber.Country.CodeSymbol;
            Parameters.Currency = viewModel.LoanAmount.Currency.Code;
            Parameters.CountryCode = viewModel.OrganisationNumber.Country.CodeSymbol;
            Parameters.OrganisationNumber = viewModel.OrganisationNumber.ToString();
        }
    }

    public class CreditElasticSearchParameters
    {
        public string CountryCode { get; set; }
        public string Currency { get; set; }
        public string OrganisationNumber { get; set; }
    }
}