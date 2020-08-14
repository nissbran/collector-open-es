using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Commands;

namespace DevOpen.Application.Handlers.Commands.Credits
{
    public class RegisterCreditHandler : CommandHandler<RegisterCredit>
    {
        private readonly ICreditAggregateStore _repository;

        public RegisterCreditHandler(ICreditAggregateStore repository)
        {
            _repository = repository;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterCredit command)
        {
            var credit = new Credit(command.Id, command);

            await _repository.Save(credit);
            
            return CommandExecutionResult.Ok;
        }
    }
}