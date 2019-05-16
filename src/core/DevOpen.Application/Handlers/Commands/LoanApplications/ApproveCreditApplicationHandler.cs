using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications.Commands;

namespace DevOpen.Application.Handlers.Commands.LoanApplications
{
    public class ApproveCreditApplicationHandler : CommandHandler<ApproveLoanApplication>
    {
        private readonly ILoanApplicationRepository _repository;

        public ApproveCreditApplicationHandler(ILoanApplicationRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(ApproveLoanApplication command)
        {
            var application = await _repository.GetById(command.Id);
            
            application.Approve();

            await _repository.Save(application);
            
            return CommandExecutionResult.Ok;
        }
    }
}