using System.Threading.Tasks;
using DevOpen.Infrastructure.Configuration;
using EventStore.ClientAPI;

namespace DevOpen.Infrastructure.Persistence.EventStore
{
    public interface IEventStoreConnectionProvider
    {
        IEventStoreConnection Connection { get; }

        Task Connect();
    }

    public class EventStoreConnectionProvider : IEventStoreConnectionProvider
    {
        public EventStoreConnectionProvider()
        {
            Connection = EventStoreConnectionFactory.Create(
                "ConnectTo=tcp://localhost:1113",
                "admin", "changeit");
        }

        public IEventStoreConnection Connection { get; }

        public async Task Connect()
        {
            await Connection.ConnectAsync();
        }
    }
}