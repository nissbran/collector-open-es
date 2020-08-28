using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Commands;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Commands;
using DevOpen.Infrastructure.Persistence.Elastic;
using DevOpen.Infrastructure.Persistence.EventStore;
using DevOpen.Infrastructure.Serialization.Resolvers;
using DevOpen.ReadModel.Credits;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.LoanApplications;
using DevOpen.ReadModel.Search;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

namespace DevOpen.Hosts.Api
{
    class Program
    {
        private static readonly JsonSerializerSettings SerializerSettings;

        static Program()
        {
            SerializerSettings = new JsonSerializerSettings();
            SerializerSettings.ContractResolver = new EventJsonContractResolver();
            SerializerSettings.Converters.Add(new StringEnumConverter());
        }
        
        public static async Task Main(string[] args)
        {
            // Plumbing --------------------------------------------
            var serviceProvider = Bootstrapper.GetServiceProvider();

            await serviceProvider.GetService<IEventStoreConnectionProvider>().Connect();
            
            var commandMediator = serviceProvider.GetService<CommandMediator>();
            var queryMediator = serviceProvider.GetService<QueryMediator>();
            // -----------------------------------------------------
            
            //await DemoMode(commandMediator, queryMediator);

            for (int i = 0; i < 1000; i++)
            {
                var applicationId = LoanApplicationId.NewId();

                await commandMediator.MediateCommand(new RegisterLoanApplication(applicationId)
                {
                    OrganisationNumber = new OrganisationNumber("5561682518", Country.Sweden),
                    RequestedAmount = Money.Create(100000, Currency.SEK),
                    VisitingAddress = new Address("Demogatan 1", string.Empty, "41420", "Göteborg", "Sverige", string.Empty)
                });
                await commandMediator.MediateCommand(new ApproveLoanApplication(applicationId));
            }

            var sw = Stopwatch.StartNew();
            var creditsForCountry = await queryMediator.MediateQuery(new GetCreditsByCountry(Country.Sweden));
            Log.Information("Fetched {Count} 'GetCreditsByCountry' in {Elapsed} ms", creditsForCountry.Count, sw.ElapsedMilliseconds);
            sw.Restart();
            creditsForCountry = await queryMediator.MediateQuery(new GetCreditsByCountry(Country.Sweden));
            Log.Information("Fetched {Count} 'GetCreditsByCountry' in {Elapsed} ms", creditsForCountry.Count, sw.ElapsedMilliseconds);
            sw.Restart();
            creditsForCountry = await queryMediator.MediateQuery(new GetCreditsByCountry(Country.Sweden));
            Log.Information("Fetched {Count} 'GetCreditsByCountry' in {Elapsed} ms", creditsForCountry.Count, sw.ElapsedMilliseconds);
            sw.Restart();
            var engagements = await queryMediator.MediateQuery(new SearchForEngagements("123001"));
            Log.Information("Fetched {Count} 'SearchForEngagements' in {Elapsed} ms", engagements.TotalEngagements, sw.ElapsedMilliseconds);
            sw.Restart();
            engagements = await queryMediator.MediateQuery(new SearchForEngagements("123001"));
            Log.Information("Fetched {Count} 'SearchForEngagements' in {Elapsed} ms", engagements.TotalEngagements, sw.ElapsedMilliseconds);
            sw.Restart();
            engagements = await queryMediator.MediateQuery(new SearchForEngagements("123001"));
            Log.Information("Fetched {Count} 'SearchForEngagements' in {Elapsed} ms", engagements.TotalEngagements, sw.ElapsedMilliseconds);
            
            //Log.Information("count: {Count}", creditsForCountry.Count);

            var inserter = new ElasticBulkInserter();
            var credits = new List<CreditViewModel>();
            for (int i = 0; i < 4000; i++)
            {
                credits.Add(new CreditViewModel(CreditId.NewId())
                {
                    LoanAmount = Money.Create(20000, Currency.SEK),
                    CreditNumber = 123321
                });
            }
            sw.Restart();
            inserter.BulkInsert(credits);
            Log.Information("Inserted 4000 credits in {Elapsed} ms", sw.ElapsedMilliseconds);
        }

        private static async Task DemoMode(CommandMediator commandMediator, QueryMediator queryMediator)
        {
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

            // Get data (Direct ES query)
            var loanApplicationView = await queryMediator.MediateQuery(new GetLoanApplicationById(applicationId));

            Log.Information("loanApplicationView: {View}", JsonConvert.SerializeObject(loanApplicationView, Formatting.Indented, SerializerSettings));


            // Approve application - Process + Builder
            await commandMediator.MediateCommand(new ApproveLoanApplication(applicationId));


            // Get data again (Direct ES query)
            var loanApplicationView2 = await queryMediator.MediateQuery(new GetLoanApplicationById(applicationId));

            Log.Information("loanApplicationView: {View}", JsonConvert.SerializeObject(loanApplicationView2, Formatting.Indented, SerializerSettings));

            await Task.Delay(2000);

            // Credits for org number (ReadModel)
            var credits = await queryMediator.MediateQuery(new GetCreditsByOrganisationNumber(organisationNumber));

            // Get credit data (Direct ES query)
            var creditView = await queryMediator.MediateQuery(new GetCreditById(credits.FirstOrDefault()));

            Log.Information("creditView: {View}", JsonConvert.SerializeObject(creditView, Formatting.Indented, SerializerSettings));


            // Register disbursement payout
            await commandMediator.MediateCommand(new RegisterDisbursementPayout(credits.FirstOrDefault(), Money.Create(100000, Currency.SEK)));


            // Get credit again data (Direct ES query)
            var creditView2 = await queryMediator.MediateQuery(new GetCreditById(credits.FirstOrDefault()));

            Log.Information("creditView: {View}", JsonConvert.SerializeObject(creditView2, Formatting.Indented, SerializerSettings));
        }
    }
}