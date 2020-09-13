using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Processes;
using DevOpen.Framework.Domain;

namespace DevOpen.Framework.Application.Mediators
{
    public class ProcessManagerMediator
    {
        private readonly IProcessedEventsStorage _processedEventsStorage;
        private readonly List<IProcessManager> _processManagers;

        public ProcessManagerMediator(IEnumerable<IProcessManager> processManagers, IProcessedEventsStorage processedEventsStorage)
        {
            _processedEventsStorage = processedEventsStorage;
            _processManagers = processManagers.ToList();
        }
        
        public async Task MediateEvent(DomainEvent domainEvent)
        {
            foreach (var processManager in _processManagers.Where(processManager => processManager.CanProcess(domainEvent)))
            {
                if (await _processedEventsStorage.Exists(processManager.ProcessName, domainEvent.GetType().Name, domainEvent.EventId))
                    continue;
                
                await processManager.Process(domainEvent);
                
                await _processedEventsStorage.Add(processManager.ProcessName, domainEvent.GetType().Name, domainEvent.EventId);
            }
        }
    }
}