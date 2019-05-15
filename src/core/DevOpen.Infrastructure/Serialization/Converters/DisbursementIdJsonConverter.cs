using System;
using DevOpen.Domain.Model;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization.Converters
{
    public class DisbursementIdJsonConverter : JsonConverter<DisbursementId>
    {
        public override void WriteJson(JsonWriter writer, DisbursementId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override DisbursementId ReadJson(JsonReader reader, Type objectType, DisbursementId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DisbursementId.Parse(reader.Value as string);
        }
    }
}