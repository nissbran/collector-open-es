using System;
using DevOpen.Domain.Model;
using DevOpen.Infrastructure.Serialization.Converters;
using Newtonsoft.Json.Serialization;

namespace DevOpen.Infrastructure.Serialization.Resolvers
{
    public class EventJsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            CreateDomainValueObjectResolvers(ref contract, objectType);

            return contract;
        }
        
        private static void CreateDomainValueObjectResolvers(ref JsonContract contract, Type objectType)
        {
            if (objectType == typeof(Money))
                contract.Converter = new MoneyJsonConverter();
            if (objectType == typeof(Currency))
                contract.Converter = new CurrencyJsonConverter();
            if (objectType == typeof(Country))
                contract.Converter = new CountryJsonConverter();
        }
    }
}