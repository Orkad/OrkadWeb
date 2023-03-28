using NHibernate;

namespace OrkadWeb.Application.Common.Behaviors
{
    public sealed class TransactionnalBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly ISession session;

        public TransactionnalBehavior(ISession session)
        {
            this.session = session;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var currentTransaction = session.GetCurrentTransaction();
            if (currentTransaction != null && currentTransaction.IsActive)
            {
                return await next();
            }
            using var tx = session.BeginTransaction();
            try
            {
                var result = await next();
                await tx.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await tx.RollbackAsync(cancellationToken);
                session.Clear();
                throw;
            }
        }
    }
}
