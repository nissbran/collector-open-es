using System;
using Elasticsearch.Net;
using Nest;

namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public class ElasticConnectionProvider
    {
        public IElasticClient Client { get; }
        
        public ElasticConnectionProvider()
        {
            var pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
            var settings = new ConnectionSettings(pool);
            Client = new ElasticClient(settings);
        }
    }
}