using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories.Aggregates;
using DevOpen.Domain;
using DevOpen.Domain.Commands;

namespace DevOpen.Application.Handlers.Commands.CreditApplications
{
    public class RegisterCreditApplicationHandler : CommandHandler<RegisterCreditApplication>
    {
        private readonly ICreditApplicationRepository _repository;

        public RegisterCreditApplicationHandler(ICreditApplicationRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterCreditApplication command)
        {
            var newApplication = new CreditApplication(command.Id, command);

            await _repository.Save(newApplication);
            
            return CommandExecutionResult.Ok;
        }
    }
}