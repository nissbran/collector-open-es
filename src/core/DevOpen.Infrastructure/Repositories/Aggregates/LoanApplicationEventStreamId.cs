using DevOpen.Domain.Model;
using DevOpen.Infrastructure.Persistence.EventStore;

namespace DevOpen.Infrastructure.Repositories.Aggregates
{
    public sealed class LoanApplicationEventStreamId : EventStreamId
    {
        private readonly string _id;

        public override string StreamName => $"LoanApplication-{_id}";

        public LoanApplicationEventStreamId(LoanApplicationId id)
        {
            _id = id.ToString();
        }
    }
}