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
            if (request is ICommand<TResponse>)
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var response = await next();
                        await transaction.CommitAsync();
                        return response;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            return await next();
        }
    }
}
