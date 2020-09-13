using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Handlers;
using DevOpen.Framework.Application.ReadModels;

namespace DevOpen.Framework.Application.Mediators
{
    public class QueryMediator
    {
        private readonly IDictionary<Type, IQueryHandler> _queryHandlers;

        public QueryMediator(IEnumerable<IQueryHandler> queryHandlers)
        {
            _queryHandlers = queryHandlers.ToDictionary(handler => handler.QueryType);
        }
        
        public async Task<TResult> MediateQuery<TResult>(Query<TResult> query)
            where TResult : class 
        {
            var queryHandler = GetQueryHandlerFor<TResult>(query.GetType());
            
            return await queryHandler.Handle(query);
        }
        
        private IQueryHandler<TResult> GetQueryHandlerFor<TResult>(Type query)
            where TResult : class 
        {
            if (!_queryHandlers.TryGetValue(query, out var queryHandler))
                throw new ArgumentException($"Query handler for {query.Name} could not be found.");

            return (IQueryHandler<TResult>)queryHandler;
        }
    }
}