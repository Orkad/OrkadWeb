using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ITransaction transaction;
        private readonly ISession session;

        public UnitOfWork(ISession session)
        {
            var currentTransaction = session.GetCurrentTransaction();
            // in case of nested context, only the last one can commit
            if (currentTransaction?.IsActive != true)
            {
                transaction = session.BeginTransaction();
            }

            this.session = session;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await session.FlushAsync(cancellationToken);
            if (transaction != null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task CancelChangesAsync(CancellationToken cancellationToken)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
        }
    }
}
