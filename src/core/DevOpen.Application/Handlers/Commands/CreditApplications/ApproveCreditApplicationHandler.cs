using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories.Aggregates;
using DevOpen.Domain.Commands;

namespace DevOpen.Application.Handlers.Commands.CreditApplications
{
    public class ApproveCreditApplicationHandler : CommandHandler<ApproveCreditApplication>
    {
        private readonly ICreditApplicationRepository _repository;

        public ApproveCreditApplicationHandler(ICreditApplicationRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(ApproveCreditApplication command)
        {
            var application = await _repository.GetById(command.Id);
            
            application.Approve();

            await _repository.Save(application);
            
            return CommandExecutionResult.Ok;
        }
    }
}