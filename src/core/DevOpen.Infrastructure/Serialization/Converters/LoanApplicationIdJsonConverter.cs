using System;
using DevOpen.Domain.Model.LoanApplications;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Serialization.Converters
{
    public class LoanApplicationIdJsonConverter : JsonConverter<LoanApplicationId>
    {
        public override void WriteJson(JsonWriter writer, LoanApplicationId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override LoanApplicationId ReadJson(JsonReader reader, Type objectType, LoanApplicationId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return LoanApplicationId.Parse(reader.Value as string);
        }
    }
}