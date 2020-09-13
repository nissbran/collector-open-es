using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications.Commands;
using DevOpen.Framework.Application.Handlers;
using DevOpen.Framework.Application.Mediators;

namespace DevOpen.Application.Handlers.Commands.LoanApplications
{
    public class DenyLoanApplicationHandler : CommandHandler<DenyLoanApplication>
    {
        private readonly ILoanApplicationAggregateStore _aggregateStore;

        public DenyLoanApplicationHandler(ILoanApplicationAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }
        
        public override async Task<CommandExecutionResult> Handle(DenyLoanApplication command)
        {
            var application = await _aggregateStore.GetById(command.Id);
            
            application.Deny();

            await _aggregateStore.Save(application);
            
            return CommandExecutionResult.Ok;
        }
    }
}