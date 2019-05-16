using System;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.Infrastructure.Serialization.Converters;
using Newtonsoft.Json.Serialization;

namespace DevOpen.Infrastructure.Serialization.Resolvers
{
    public class EventJsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            CreateDomainIdsResolvers(ref contract, objectType);
            CreateDomainValueObjectResolvers(ref contract, objectType);

            return contract;
        }
        
        private static void CreateDomainIdsResolvers(ref JsonContract contract, Type objectType)
        {
            if (objectType == typeof(DisbursementId))
                contract.Converter = new DisbursementIdJsonConverter();
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