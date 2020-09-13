using System;

namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public abstract class ElasticView
    {
        public Guid Id { get; set; }
        
        public string ViewType { get; set; }
        
        public string JsonData { get; set; }
    }
}