using System;
using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Domain;

namespace DevOpen.Application.Handlers
{
    public abstract class CommandHandler<T> : ICommandHandler<T>
        where T : Command
    {
        public Type CommandType { get; } = typeof(T);

        public async Task<CommandExecutionResult> Handle(Command command)
        {
            return await Handle((T)command);
        }

        public abstract Task<CommandExecutionResult> Handle(T command);
    }
}