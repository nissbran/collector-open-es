using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.Credits.Commands;
using DevOpen.Framework.Application.Handlers;
using DevOpen.Framework.Application.Mediators;

namespace DevOpen.Application.Handlers.Commands.Credits
{
    public class RegisterDisbursementPayoutHandler : CommandHandler<RegisterDisbursementPayout>
    {
        private readonly ICreditAggregateStore _repository;

        public RegisterDisbursementPayoutHandler(ICreditAggregateStore repository)
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