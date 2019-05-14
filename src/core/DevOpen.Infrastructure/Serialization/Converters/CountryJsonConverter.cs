using System;
using DevOpen.Domain.Model;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization.Converters
{
    public class CountryJsonConverter : JsonConverter<Country>
    {
        public override void WriteJson(JsonWriter writer, Country value, JsonSerializer serializer)
        {
            writer.WriteValue(value.CodeSymbol);
        }

        public override Country ReadJson(JsonReader reader, Type objectType, Country existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Country.Parse(reader.Value as string);
        }
    }
}