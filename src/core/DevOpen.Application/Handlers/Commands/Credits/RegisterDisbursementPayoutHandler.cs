using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.Credits.Commands;

namespace DevOpen.Application.Handlers.Commands.Credits
{
    public class RegisterDisbursementPayoutHandler : CommandHandler<RegisterDisbursementPayout>
    {
        private readonly ICreditRootRepository _repository;

        public RegisterDisbursementPayoutHandler(ICreditRootRepository repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterDisbursementPayout command)
        {
            var credit = await _repository.GetById(command.Id);
            
            credit.RegisterDisbursementPayout(command.Amount);

            await _repository.Save(credit);
            
            return CommandExecutionResult.Ok;
        }
    }
}