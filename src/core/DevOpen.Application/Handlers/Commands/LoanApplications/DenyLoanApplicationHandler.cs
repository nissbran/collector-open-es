using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.LoanApplications.Commands;

namespace DevOpen.Application.Handlers.Commands.LoanApplications
{
    public class DenyLoanApplicationHandler : CommandHandler<DenyLoanApplication>
    {
        private readonly ILoanApplicationRepository _repository;

        public DenyLoanApplicationHandler(ILoanApplicationRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(DenyLoanApplication command)
        {
            var application = await _repository.GetById(command.Id);
            
            application.Deny();

            await _repository.Save(application);
            
            return CommandExecutionResult.Ok;
        }
    }
}