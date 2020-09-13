using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Processes;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Mediators
{
    public class ProcessManagerMediator
    {
        private readonly List<IProcessManager> _processManagers;

        public ProcessManagerMediator(IEnumerable<IProcessManager> processManagers)
        {
            _processManagers = processManagers.ToList();
        }
        
        public async Task MediateEvent(DomainEvent domainEvent)
        {
            foreach (var processManager in _processManagers)
            {
                await processManager.Handle(domainEvent);
            }
        }
    }
}