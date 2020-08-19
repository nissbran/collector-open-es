using System;
using System.Collections.Generic;
using DevOpen.ReadModel.Credits.Model;
using Nest;

namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public class ElasticBulkInserter : ElasticViewStore
    {
        public void BulkInsert(List<CreditViewModel> creditViewModels)
        {
            var bulkAllObservable = Client.BulkAll(creditViewModels, b => b
                    .Index("credits2")
                    .BackOffTime("30s") 
                    .BackOffRetries(2) 
                    .RefreshOnCompleted()
                    .MaxDegreeOfParallelism(Environment.ProcessorCount)
                    .Size(1000) 
                )
                .Wait(TimeSpan.FromMinutes(15), next => 
                {
                    // do something e.g. write number of pages to console
                });
        }
    }
}