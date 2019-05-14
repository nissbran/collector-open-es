using System;
using System.Threading;

namespace DevOpen.Infrastructure.Subscriptions.EventStore
{
    public class SubscriptionCheckpoint
    {
        public Guid SubscriptionId { get; }
        
        public long LastProcessedPosition { get; private set; }
        
        public DateTime UpdatedUtc { get; private set; }

        public long EventsProcessed => _eventsProcessed;

        private long _eventsProcessed;

        public SubscriptionCheckpoint(Guid subscriptionId, long lastProcessedPosition, long eventsProcessed, DateTime updatedUtc)
        {
            SubscriptionId = subscriptionId;
            LastProcessedPosition = lastProcessedPosition;
            _eventsProcessed = eventsProcessed;
            UpdatedUtc = updatedUtc;
        }
        
        public void IncrementEventsProcessed()
        {
            Interlocked.Increment(ref _eventsProcessed);
        }

        public void SetLastProcessedPosition(long lastProcessedPosition)
        {
            LastProcessedPosition = lastProcessedPosition;
            UpdatedUtc = DateTime.UtcNow;
        }
    }
}