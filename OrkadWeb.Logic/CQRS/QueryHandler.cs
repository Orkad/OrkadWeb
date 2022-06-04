using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.CQRS
{
    /// <summary>
    /// Base class for handling queries.
    /// </summary>
    /// <typeparam name="TCommand">Query type to handle.</typeparam>
    /// <typeparam name="TResponse">Response of query type to handle.</typeparam>
    public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
    {
        /// <inheritdoc/>
        public Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken) => HandleQuery(query, cancellationToken);

        /// <summary>
        /// Handles a query.
        /// </summary>
        /// <param name="command">Query to handle.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Response from the query.</returns>
        protected abstract Task<TResponse> HandleQuery(TQuery query, CancellationToken cancellationToken);
    }

    /// <inheritdoc/>
    public abstract class QueryHandler<TQuery> : QueryHandler<TQuery, Unit> where TQuery : IQuery
    {

    }
}
