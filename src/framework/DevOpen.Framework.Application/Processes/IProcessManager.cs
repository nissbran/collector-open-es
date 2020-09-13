using System.Threading.Tasks;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Processes
{
    public interface IProcessManager 
    {
        string ProcessName { get; }

        bool CanProcess(DomainEvent domainEvent);

        Task Process(DomainEvent domainEvent);
    }
}