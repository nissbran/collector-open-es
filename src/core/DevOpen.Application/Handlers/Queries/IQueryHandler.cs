using System;
using System.Threading.Tasks;
using DevOpen.Domain;
using DevOpen.ReadModel;

namespace DevOpen.Application.Handlers.Queries
{  
    public interface IQueryHandler
    {
        Type QueryType { get; }
    }

    public interface IQueryHandler<TResult> : IQueryHandler
    {
        Task<TResult> Handle(Query query);
    }
    
    public interface IQueryHandler<in TQuery, TResult> : IQueryHandler<TResult>
        where TQuery : Query<TResult>
        where TResult : class
    {
        Task<TResult> Handle(TQuery query);
    }
}