using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Commands;
using DevOpen.Framework.Application.Handlers;
using DevOpen.Framework.Application.Mediators;

namespace DevOpen.Application.Handlers.Commands.LoanApplications
{
    public class RegisterLoanApplicationHandler : CommandHandler<RegisterLoanApplication>
    {
        private readonly ILoanApplicationAggregateStore _aggregateStore;
        private readonly ICreditNumberRepository _creditNumberRepository;

        public RegisterLoanApplicationHandler(ILoanApplicationAggregateStore aggregateStore, ICreditNumberRepository creditNumberRepository)
        {
            _aggregateStore = aggregateStore;
            _creditNumberRepository = creditNumberRepository;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterLoanApplication command)
        {
            var newApplication = new LoanApplication(command.Id, command, await _creditNumberRepository.GetNextAsync());

            await _aggregateStore.Save(newApplication);
            
            return CommandExecutionResult.Ok;
        }
    }
}