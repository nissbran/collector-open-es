using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Domain.Commands;
using DevOpen.Domain.Model;
using Microsoft.Extensions.DependencyInjection;

namespace DevOpen.Hosts.Api
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var serviceProvider = Bootstrapper.GetServiceProvider();

            var commandMediator = serviceProvider.GetService<CommandMediator>();

            var applicationId = CreditApplicationId.NewId();
            
            await commandMediator.MediateCommand(new RegisterCreditApplication(applicationId)
            {
                OrganisationNumber = new OrganisationNumber("5561682518", Country.Sweden),
                RequestedAmount = Money.Create(100000, Currency.SEK),
                VisitingAddress = new Address("Demogatan 1", string.Empty, "41420", "Göteborg", "Sverige", string.Empty)
            });

            await commandMediator.MediateCommand(new ApproveCreditApplication(applicationId));
        }
    }
}