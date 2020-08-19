namespace DevOpen.Infrastructure.Persistence.Elastic
{
    public class ElasticIndexClearer : ElasticViewStore
    {
        public void ClearIndices()
        {
            Client.Indices.Delete(ElasticIndices.Credits);
            Client.Indices.Delete(ElasticIndices.Applications);
        }
    }
}