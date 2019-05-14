using System;
using DevOpen.Domain.Model;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization.Converters
{
    public class CurrencyJsonConverter : JsonConverter<Currency>
    {
        public override void WriteJson(JsonWriter writer, Currency value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Code);
        }

        public override Currency ReadJson(JsonReader reader, Type objectType, Currency existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Currency.Parse(reader.Value as string);
        }
    }
}