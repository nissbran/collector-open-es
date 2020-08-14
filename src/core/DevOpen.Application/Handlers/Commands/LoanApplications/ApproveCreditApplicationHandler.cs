using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications.Commands;

namespace DevOpen.Application.Handlers.Commands.LoanApplications
{
    public class ApproveCreditApplicationHandler : CommandHandler<ApproveLoanApplication>
    {
        private readonly ILoanApplicationAggregateStore _aggregateStore;

        public ApproveCreditApplicationHandler(ILoanApplicationAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }
        
        public override async Task<CommandExecutionResult> Handle(ApproveLoanApplication command)
        {
            var application = await _aggregateStore.GetById(command.Id);
            
            application.Approve();

            await _aggregateStore.Save(application);
            
            return CommandExecutionResult.Ok;
        }
    }
}