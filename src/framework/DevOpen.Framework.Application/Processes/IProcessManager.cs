using System.Threading.Tasks;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Processes
{
    public interface IProcessManager 
    {
        Task Handle(DomainEvent domainEvent);
    }
}