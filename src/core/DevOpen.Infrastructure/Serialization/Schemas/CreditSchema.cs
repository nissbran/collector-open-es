using DevOpen.Domain.Model.Credits.Events;
using DevOpen.Framework.Infrastructure.Serialization.Schemas;

namespace DevOpen.Infrastructure.Serialization.Schemas
{
    public class CreditSchema : EventSchema<CreditDomainEvent>
    {
        public const string SchemaName = "Credit";

        public override string Name => SchemaName;
    }
}