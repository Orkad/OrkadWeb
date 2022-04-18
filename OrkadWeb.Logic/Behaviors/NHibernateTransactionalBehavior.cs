using MediatR;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Logic.Abstractions
{
    /// <summary>
    /// Transaction behavior between NHibernate and Mediatr
    /// </summary>
    public sealed class NHibernateTransactionalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly ISession session;

        public NHibernateTransactionalBehavior(ISession session)
        {
            this.session = session;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var response = await next();
                    // Only Commit transaction if there is no exception
                    if (request is ICommand<TResponse>)
                    {
                        await transaction.CommitAsync(cancellationToken);
                    }
                    return response;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
