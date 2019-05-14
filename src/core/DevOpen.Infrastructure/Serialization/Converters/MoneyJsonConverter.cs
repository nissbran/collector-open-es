using System;
using DevOpen.Domain.Model;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization.Converters
{
    public class MoneyJsonConverter : JsonConverter<Money>
    {
        public override void WriteJson(JsonWriter writer, Money value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToFullPrecisionString());
        }

        public override Money ReadJson(JsonReader reader, Type objectType, Money existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Money.Parse(reader.Value as string);
        }
    }
}