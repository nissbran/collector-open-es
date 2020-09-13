using System;
using DevOpen.Framework.Application.ReadModels;
using DevOpen.ReadModel;
using DevOpen.ReadModel.Credits.Model;
using DevOpen.ReadModel.LoanApplications.Model;
using Nest;

namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public class ElasticIndexNameResolver
    {
        private const string IndexPrefix = "local";
        
        public ElasticIndexNameResolver()
        {
            
        }

        public IndexName ResolveIndexName<TView>() where TView : IViewModel =>
            typeof(TView).Name switch
            {
                nameof(CreditViewModel) => $"{IndexPrefix}-credits",
                nameof(LoanApplicationViewModel) => $"{IndexPrefix}-applications",
                _ => throw new NotImplementedException("")
            };
        
    }
}