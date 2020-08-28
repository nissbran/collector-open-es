using System;

namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public abstract class ElasticView
    {
        public Guid Id { get; set; }
        
        public string ViewType { get; set; }
        
        public string JsonData { get; set; }
    }

    public class SearchableElasticView : ElasticView
    {
        public string CreditNumber { get; set; }
        public string CountryCode { get; set; }
        public string Currency { get; set; }
        public string OrganisationNumber { get; set; }
    }
}