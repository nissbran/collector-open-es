using System.Threading.Tasks;
using DevOpen.Application.Repositories;
using DevOpen.Domain.Model.Credits;
using DevOpen.Domain.Model.Credits.Commands;
using DevOpen.Framework.Application.Handlers;
using DevOpen.Framework.Application.Mediators;

namespace DevOpen.Application.Handlers.Commands.Credits
{
    public class RegisterCreditHandler : CommandHandler<RegisterCredit>
    {
        private readonly ICreditAggregateStore _aggregateStore;
        private readonly ICreditNumberRepository _creditNumberRepository;

        public RegisterCreditHandler(ICreditAggregateStore aggregateStore, ICreditNumberRepository creditNumberRepository)
        {
            _aggregateStore = aggregateStore;
            _creditNumberRepository = creditNumberRepository;
        }
        
        public override async Task<CommandExecutionResult> Handle(RegisterCredit command)
        {
            var credit = new Credit(command.Id, command, command.CreditNumber ?? await _creditNumberRepository.GetNextAsync());

            await _aggregateStore.Save(credit);
            
            return CommandExecutionResult.Ok;
        }
    }
}