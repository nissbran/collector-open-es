using System.Threading.Tasks;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.LoanApplications.Model;
using Nest;

namespace DevOpen.Infrastructure.Storage
{
    public class ApplicationViewElasticStore : ElasticViewStore, IApplicationViewStore
    {
        public ApplicationViewElasticStore() : base(settings => settings.DefaultIndex(ElasticIndices.Applications))
        {
        }
        
        public async Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId)
        {
            var response = await Client.GetAsync<SearchableElasticView>(applicationId.ToString());

            return response.Found ? Serializer.Deserialize<LoanApplicationViewModel>(response.Source.JsonData) : null;
        }

        public async Task Upsert(LoanApplicationViewModel viewModel)
        {
            await Client.IndexAsync(new IndexRequest<SearchableElasticView>(CreateElasticView(viewModel), ElasticIndices.Applications, viewModel.Id.ToString()));
        }

        private SearchableElasticView CreateElasticView(LoanApplicationViewModel viewModel)
        {
            var elasticView = new SearchableElasticView
            {
                Id = viewModel.Id, 
                ViewType = nameof(LoanApplicationViewModel),
                JsonData = Serializer.Serialize(viewModel),
                CreditNumber = viewModel.CreditNumber.ToString(),
                Currency = viewModel.RequestedAmount.Currency.Code,
                CountryCode = viewModel.OrganisationNumber.Country.CodeSymbol,
                OrganisationNumber = viewModel.OrganisationNumber.ToString()
            };
            return elasticView;
        }
    }
}