using System;
using DevOpen.Domain.Model.Credits;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization.Converters
{
    public class CreditIdJsonConverter : JsonConverter<CreditId>
    {
        public override void WriteJson(JsonWriter writer, CreditId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override CreditId ReadJson(JsonReader reader, Type objectType, CreditId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return CreditId.Parse(reader.Value as string);
        }
    }
}