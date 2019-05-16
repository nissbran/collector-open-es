using System.Threading.Tasks;
using DevOpen.Domain;

namespace DevOpen.Application.Processes
{
    public interface IProcessManager 
    {
        Task Handle(DomainEvent domainEvent);
    }
}