using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Nest;

namespace ElasticTest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var uris = new[]
            {
                new Uri("http://localhost:9200"),
                new Uri("http://localhost:9201"),
                new Uri("http://localhost:9202"),
            };

            //var connectionPool = new SniffingConnectionPool(uris);
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DefaultIndex("guid");

            var client = new ElasticClient(settings);

           // var response = await client.CountAsync(new CountRequest());

            var person = new Person
            {
                Id = Guid.Parse("2fe150af-47a6-4dd9-9607-156f2391a9e4"),
                FirstName = "Martijn3",
                LastName = "Laarman"
            };

            //var indexResponse = client.IndexDocument(person); 

            var response2 = await client.GetAsync<Person>(DocumentPath<Person>.Id(new Id(Guid.Parse("2fe150af-47a6-4dd9-9607-156f2391a9e4").ToString())));

            var personToUpdate = response2.Source;
            personToUpdate.LastName = "Test";
            
            var asyncIndexResponse = await client.IndexDocumentAsync(personToUpdate); 


        }
    }
        
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}