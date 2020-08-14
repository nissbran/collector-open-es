using System;

namespace DevOpen.Infrastructure.Storage
{
    public abstract class ElasticView
    {
        public Guid Id { get; set; }
        
        public string JsonData { get; set; }
    }

    public abstract class SearchableElasticView : ElasticView
    {
        
    }
}