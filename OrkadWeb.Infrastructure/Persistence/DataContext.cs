using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    internal class DataContext : IDataContext
    {
        private readonly ITransaction transaction;

        public DataContext(ITransaction transaction)
        {
            this.transaction = transaction;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
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
