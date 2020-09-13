using EventStore.ClientAPI;

namespace DevOpen.Framework.Infrastructure.Subscriptions
{
    public class CatchAllSubscriptionSettings : SubscriptionSettings
    {
        public long LastCommitPosition { get; set; } = Position.Start.CommitPosition;
    }
}