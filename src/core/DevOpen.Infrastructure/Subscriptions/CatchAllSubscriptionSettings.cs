using EventStore.ClientAPI;

namespace DevOpen.Infrastructure.Subscriptions
{
    public class CatchAllSubscriptionSettings : SubscriptionSettings
    {
        public long LastCommitPosition { get; set; } = Position.Start.CommitPosition;
    }
}