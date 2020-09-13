using System;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Mediators;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Handlers
{
    public interface ICommandHandler
    {
        Type CommandType { get; }
        
        Task<CommandExecutionResult> Handle(Command command);
    }
    
    public interface ICommandHandler<in T> : ICommandHandler
        where T : Command
    {
        Task<CommandExecutionResult> Handle(T command);
    }
}