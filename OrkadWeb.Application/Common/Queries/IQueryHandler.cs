using MediatR;

namespace OrkadWeb.Domain.Common
{
    /// <summary>
    /// Handle the specified query. 
    /// </summary>
    /// <typeparam name="TQuery">Type of the query</typeparam>
    /// <typeparam name="TResponse">Type of the query response</typeparam>
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : IQuery<TResponse> { }
}
