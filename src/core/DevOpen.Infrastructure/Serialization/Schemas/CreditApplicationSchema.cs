using DevOpen.Domain.Events;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public class CreditApplicationSchema : EventSchema<CreditApplicationDomainEvent>
    {
        public const string SchemaName = "CreditApplication";

        public override string Name => SchemaName;
    }
}