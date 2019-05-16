using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Domain;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Commands;
using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.ReadModel.LoanApplications;
using Serilog;

namespace DevOpen.Application.Processes
{
    public class LoanApplicationProcessManager : IProcessManager
    {
        private readonly CommandMediator _commandMediator;
        private readonly QueryMediator _queryMediator;

        public LoanApplicationProcessManager(CommandMediator commandMediator, QueryMediator queryMediator)
        {
            _commandMediator = commandMediator;
            _queryMediator = queryMediator;
        }
        
        public async Task Handle(DomainEvent domainEvent)
        {
            var loanApplicationEvent = domainEvent as LoanApplicationDomainEvent;
            
            switch (loanApplicationEvent)
            {
                case ApplicationApproved applicationApproved:
                    
                    var application = await _queryMediator.MediateQuery(new GetLoanApplicationByIdQuery(applicationApproved.ApplicationId));

                    var creditId = CreditId.NewId();
                    
                    Log.Information("Processing {Event}, creating new credit with id {Id}", nameof(ApplicationApproved), creditId);
                    
                    await _commandMediator.MediateCommand(new RegisterCredit(creditId, applicationApproved.ApplicationId)
                    {
                        OrganisationNumber = application.OrganisationNumber,
                        LoanAmount = application.RequestedAmount
                    });
                    
                    break;
            }
        }
    }
}