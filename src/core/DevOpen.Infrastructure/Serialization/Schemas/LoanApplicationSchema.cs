using DevOpen.Domain.Model.LoanApplications.Events;
using DevOpen.Framework.Infrastructure.Serialization.Schemas;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public class LoanApplicationSchema : EventSchema<LoanApplicationDomainEvent>
    {
        public const string SchemaName = "LoanApplication";

        public override string Name => SchemaName;
    }
}