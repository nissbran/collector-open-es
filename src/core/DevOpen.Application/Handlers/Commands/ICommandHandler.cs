using System;
using System.Threading.Tasks;
using DevOpen.Application.Mediators;
using DevOpen.Domain;

namespace DevOpen.Application.Handlers
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