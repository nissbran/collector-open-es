using DevOpen.Domain.Model;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public sealed class CreditEventStreamId : EventStreamId
    {
        private readonly string _id;

        public override string StreamName => $"Credit-{_id}";

        public CreditEventStreamId(CreditId id)
        {
            _id = id.ToString();
        }
    }
}