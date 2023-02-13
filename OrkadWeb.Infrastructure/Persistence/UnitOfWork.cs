using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ITransaction transaction;

        public UnitOfWork(ISession session)
        {
            transaction = session.BeginTransaction();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await transaction.CommitAsync(cancellationToken);
        }

        public async Task CancelChangesAsync(CancellationToken cancellationToken)
        {
            await transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            transaction.Dispose();
        }
    }
}
