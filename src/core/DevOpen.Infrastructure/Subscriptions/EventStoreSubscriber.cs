using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace DevOpen.Infrastructure.Subscriptions.EventStore
{
    public abstract class EventStoreSubscriber
    {
        protected IEventStoreConnection Connection { get; }

        protected EventStoreSubscriber(IEventStoreConnection connection)
        {
            Connection = connection;
            
            SetupConnectionEventListeners();
        }

        private void SetupConnectionEventListeners()
        {
            Connection.Connected += OnConnected;
            Connection.Disconnected += OnDisconnected;
            Connection.Closed += OnConnectionClosed;
        }

        protected virtual bool IsValidEvent(ResolvedEvent resolvedEvent)
        {
            return resolvedEvent.Event != null &&
                   !resolvedEvent.Event.EventType.StartsWith("$");
        }

        protected abstract Task EventAppeared(ResolvedEvent resolvedEvent, long currentPosition);

        protected virtual void OnConnected(object sender, ClientConnectionEventArgs clientConnectionEventArgs)
        {
        }

        protected virtual void OnDisconnected(object sender, ClientConnectionEventArgs clientConnectionEventArgs)
        {
        }

        protected virtual void OnConnectionClosed(object sender, ClientClosedEventArgs clientClosedEventArgs)
        {
        }
    }
}