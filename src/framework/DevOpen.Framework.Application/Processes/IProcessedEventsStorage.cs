using System;
using System.Threading.Tasks;

namespace DevOpen.Framework.Application.Processes
{
    public interface IProcessedEventsStorage
    {
        public Task<bool> Exists(string processName, string eventType, Guid eventId);
        public Task Add(string processName, string eventType, Guid eventId);
    }
}