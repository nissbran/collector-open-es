namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public class SearchableElasticView : ElasticView
    {
        public string CreditNumber { get; set; }
        public string CountryCode { get; set; }
        public string Currency { get; set; }
        public string OrganisationNumber { get; set; }
    }
}