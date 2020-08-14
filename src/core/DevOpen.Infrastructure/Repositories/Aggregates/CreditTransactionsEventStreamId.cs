using DevOpen.Domain.Model.Credits;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public sealed class CreditTransactionsEventStreamId : EventStreamId
    {
        private readonly string _id;

        public override string StreamName => $"Credit-{_id}-Transactions";

        public CreditTransactionsEventStreamId(CreditId id)
        {
            _id = id.ToString();
        }
    }
}