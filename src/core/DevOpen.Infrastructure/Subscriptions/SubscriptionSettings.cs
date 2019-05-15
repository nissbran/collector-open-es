namespace DevOpen.Infrastructure.Subscriptions
{
    public class SubscriptionSettings
    {
        public int MaxRetryAttempts { get; set; } = 0;
        public bool StopOnException { get; set; } = false;
        public int StopSubscriptionTimeout { get; set; } = 60000;
        public int ReadBatchSize { get; set; } = 200;
        public bool ResolveLinkEvents { get; set; } = false;
    }
}