using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Commands;

namespace DevOpen.Application.Handlers.Commands.LoanApplication
{
    public class RegisterLoanApplicationHandler : CommandHandler<RegisterLoanApplication>
    {
        private readonly ILoanApplicationRepository _repository;

        public RegisterLoanApplicationHandler(ILoanApplicationRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterLoanApplication command)
        {
            var newApplication = new Domain.LoanApplication(command.Id, command);

            await _repository.Save(newApplication);
            
            return CommandExecutionResult.Ok;
        }
    }
}