namespace DevOpen.Framework.Application.ReadModels
{
    public abstract class Query
    {
    }
    
    public abstract class Query<TResult> : Query where TResult : class      
    {
    }
}