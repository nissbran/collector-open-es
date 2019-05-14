using System;
using System.Threading.Tasks;
using DevOpen.Domain;

namespace DevOpen.Application.Handlers.Queries
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : Query<TResult>
        where TResult : class 
    {
        public abstract Task<TResult> Handle(TQuery query);

        public async Task<TResult> Handle(Query query)
        {
            return await Handle((TQuery) query);
        }

        public Type QueryType { get; } = typeof(TQuery);
    }
}