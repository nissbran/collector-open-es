using System;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Mediators;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Handlers
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