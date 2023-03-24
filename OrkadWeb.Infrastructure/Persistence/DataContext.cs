using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    internal class DataContext : IDataContext
    {
        private readonly ITransaction transaction;

        public DataContext(ISession session, IRepository repository)
        {
            var currentTransaction = session.GetCurrentTransaction();
            // in case of nested context, only the last one can commit
            if (currentTransaction?.IsActive != true)
            {
                transaction = session.BeginTransaction();
            }
            Repository = repository;
        }

        public IRepository Repository { get; }

        public async Task Commit(CancellationToken cancellationToken)
        {
            if (transaction != null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
        }
    }
}
