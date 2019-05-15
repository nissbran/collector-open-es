using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Commands;

namespace DevOpen.Application.Handlers.Commands.Credits
{
    public class InitializeDisbursementPayoutHandler : CommandHandler<InitializeDisbursementPayout>
    {
        private readonly ICreditRootRepository _repository;

        public InitializeDisbursementPayoutHandler(ICreditRootRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(InitializeDisbursementPayout command)
        {
            var credit = await _repository.GetById(command.Id);
            
            credit.InitializeDisbursementPayout(command.Amount);

            await _repository.Save(credit);
            
            return CommandExecutionResult.Ok;
        }
    }
}