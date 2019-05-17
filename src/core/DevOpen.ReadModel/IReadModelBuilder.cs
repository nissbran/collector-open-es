using System.Threading.Tasks;
using DevOpen.Domain;

namespace DevOpen.ReadModel
{
    public interface IReadModelBuilder
    {
        Task Handle(DomainEvent domainEvent);
    }
}