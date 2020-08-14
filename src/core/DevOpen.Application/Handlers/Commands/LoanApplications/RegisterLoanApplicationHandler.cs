using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Domain.Model.LoanApplications.Commands;

namespace DevOpen.Application.Handlers.Commands.LoanApplications
{
    public class RegisterLoanApplicationHandler : CommandHandler<RegisterLoanApplication>
    {
        private readonly ILoanApplicationAggregateStore _aggregateStore;

        public RegisterLoanApplicationHandler(ILoanApplicationAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterLoanApplication command)
        {
            var newApplication = new LoanApplication(command.Id, command);

            await _aggregateStore.Save(newApplication);
            
            return CommandExecutionResult.Ok;
        }
    }
}