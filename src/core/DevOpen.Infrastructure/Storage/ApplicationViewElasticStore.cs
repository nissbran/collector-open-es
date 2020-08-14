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
        private const string Index = "applications";

        public ApplicationViewElasticStore() : base(Index)
        {
        }
        
        public async Task<LoanApplicationViewModel> GetById(LoanApplicationId applicationId)
        {
            var response = await Client.GetAsync<CreditElasticView>(applicationId.ToString());

            return response.Found ? Serializer.Deserialize<LoanApplicationViewModel>(response.Source.JsonData) : null;
        }

        public async Task Upsert(LoanApplicationViewModel viewModel)
        {
            await Client.IndexAsync(new IndexRequest<ApplicationElasticView>(CreateElasticView(viewModel), Index, viewModel.Id.ToString()));
        }

        private ApplicationElasticView CreateElasticView(LoanApplicationViewModel viewModel)
        {
            var elasticView = new ApplicationElasticView
            {
                Id = viewModel.Id, 
                JsonData = Serializer.Serialize(viewModel)
            };
            return elasticView;
        }
    }
    
    public class ApplicationElasticView : SearchableElasticView
    {
    }
}