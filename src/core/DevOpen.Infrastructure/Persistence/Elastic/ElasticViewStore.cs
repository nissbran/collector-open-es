using System;
using System.Collections.Generic;
using DevOpen.Infrastructure.Serialization;
using DevOpen.Infrastructure.Serialization.Converters;
using DevOpen.Infrastructure.Storage;
using Elasticsearch.Net;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;

namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public abstract class ElasticViewStore
    {
        protected readonly ElasticClient Client;
        protected readonly ViewModelSerializer Serializer;

        protected ElasticViewStore(Action<ConnectionSettings> connectionSettingsOverrides = null)
        {
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(pool, (serializer, connectionSettings) => 
                new JsonNetSerializer(serializer, connectionSettings, 
                    null,null, new List<JsonConverter>
                    {
                        new CreditIdJsonConverter(), 
                        new LoanApplicationIdJsonConverter()
                    }));
            connectionSettingsOverrides?.Invoke(settings);
            Client = new ElasticClient(settings);
            Serializer = new ViewModelSerializer();
        }

        protected T ParseViewModel<T>(ElasticView elasticView) where T : class
        {
            return Serializer.Deserialize<T>(elasticView.JsonData);
        }
    }
}