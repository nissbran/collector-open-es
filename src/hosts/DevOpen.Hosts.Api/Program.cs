using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Commands;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Commands;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.LoanApplications;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpen.Hosts.Api
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Plumbing
            var serviceProvider = Bootstrapper.GetServiceProvider();

            await serviceProvider.GetService<IEventStoreConnectionProvider>().Connect();
            
            var commandMediator = serviceProvider.GetService<CommandMediator>();
            var queryMediator = serviceProvider.GetService<QueryMediator>();

            
            
            // Register a new application
            var applicationId = LoanApplicationId.NewId();
            var organisationNumber = new OrganisationNumber("5561682518", Country.Sweden);

            var registerApplicationCommand = new RegisterLoanApplication(applicationId)
            {
                OrganisationNumber = organisationNumber,
                RequestedAmount = Money.Create(100000, Currency.SEK),
                VisitingAddress = new Address("Demogatan 1", string.Empty, "41420", "Göteborg", "Sverige", string.Empty)
            };
            
            await commandMediator.MediateCommand(registerApplicationCommand);

            
            // Approve application
            await commandMediator.MediateCommand(new ApproveLoanApplication(applicationId));

            
            
            
            // Get data 
            var loanApplicationView = await queryMediator.MediateQuery(new GetLoanApplicationById(applicationId));

            
            // Credits for org number
            var credits = await queryMediator.MediateQuery(new GetCreditsByOrganisationNumber(organisationNumber));
            
            
            
            
            await commandMediator.MediateCommand(new RegisterCredit(CreditId.NewId())
            {
                LoanAmount = Money.Create(100000, Currency.SEK),
                OrganisationNumber = new OrganisationNumber("5561682518", Country.Sweden)
            });

        }
    }
}