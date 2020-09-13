using System.Threading.Tasks;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.ReadModels
{
    public interface IReadModelBuilder
    {
        Task Handle(DomainEvent domainEvent);

        void ClearModel();

        void Switch();
    }
}