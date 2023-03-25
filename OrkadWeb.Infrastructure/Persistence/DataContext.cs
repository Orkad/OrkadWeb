using NHibernate;
using OrkadWeb.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    internal class DataContext : IDataContext, IDisposable
    {
        private ITransaction transaction;
        private readonly ISession session;

        public IDataService Repository { get; }

        public DataContext(ISession session, IDataService repository)
        {
            this.session = session;
            Repository = repository;
        }

        public Task BeginTransaction(CancellationToken cancellationToken)
        {
            transaction = session.BeginTransaction();
            return Task.CompletedTask;
        }

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
