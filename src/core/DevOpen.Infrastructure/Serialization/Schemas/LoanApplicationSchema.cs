using DevOpen.Domain.Model.LoanApplications.Events;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public class LoanApplicationSchema : EventSchema<LoanApplicationDomainEvent>
    {
        public const string SchemaName = "LoanApplication";

        public override string Name => SchemaName;
    }
}