using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Mediators
{
    public class ReadModelBuilderMediator
    {
        private readonly List<IReadModelBuilder> _readModelBuilders;

        public ReadModelBuilderMediator(IEnumerable<IReadModelBuilder> readModelBuilders)
        {
            _readModelBuilders = readModelBuilders.ToList();
        }
        
        public async Task MediateEvent(DomainEvent domainEvent)
        {
            foreach (var readModelBuilder in _readModelBuilders)
            {
                await readModelBuilder.Handle(domainEvent);
            }
        }
    }
}