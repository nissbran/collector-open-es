using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Application.Handlers;
using DevOpen.Application.Handlers.Commands;
using DevOpen.Domain;

namespace DevOpen.Application.Mediators
{
    public class CommandMediator
    {
        private readonly IDictionary<Type, ICommandHandler> _commandHandlers;

        public CommandMediator(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers.ToDictionary(handler => handler.CommandType);
        }
        
        public async Task<CommandExecutionResult> MediateCommand(Command command)
        {
            var commandHandler = GetCommandHandlerFor(command.GetType());
            
            return await commandHandler.Handle(command);
        }
        
        private ICommandHandler GetCommandHandlerFor(Type command)
        {
            if (!_commandHandlers.TryGetValue(command, out var commandHandler))
                throw new ArgumentException($"Command handler for {command.Name} could not be found.");

            return commandHandler;
        }
    }
}