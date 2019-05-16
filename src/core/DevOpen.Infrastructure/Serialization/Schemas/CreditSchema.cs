using DevOpen.Domain.Model.Credits.Events;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public class CreditSchema : EventSchema<CreditDomainEvent>
    {
        public const string SchemaName = "Credit";

        public override string Name => SchemaName;
    }
}