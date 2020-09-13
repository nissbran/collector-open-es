using DevOpen.Domain.Model.LoanApplications;
using DevOpen.Framework.Infrastructure.Persistence.EventStoreDb;

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