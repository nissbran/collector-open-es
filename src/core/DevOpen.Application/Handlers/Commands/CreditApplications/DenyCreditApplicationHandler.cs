using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories.Aggregates;
using DevOpen.Domain.Commands;

namespace DevOpen.Application.Handlers.Commands.CreditApplications
{
    public class DenyCreditApplicationHandler : CommandHandler<DenyCreditApplication>
    {
        private readonly ICreditApplicationRepository _repository;

        public DenyCreditApplicationHandler(ICreditApplicationRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(DenyCreditApplication command)
        {
            var application = await _repository.GetById(command.Id);
            
            application.Deny();

            await _repository.Save(application);
            
            return CommandExecutionResult.Ok;
        }
    }
}