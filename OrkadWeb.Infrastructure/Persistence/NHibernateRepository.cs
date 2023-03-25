using NHibernate;
using NHibernate.Linq;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Infrastructure.Persistence
{
    /// <inheritdoc cref="IDataService"/>
    internal class NHibernateDataService : IDisposable, IDataService
    {
        private readonly ISession session;

        public NHibernateDataService(ISession session)
        {
            this.session = session;
        }

        /// <inheritdoc/>
        public T Get<T>(object id) => session.Get<T>(id) ?? throw new DataNotFoundException<T>(id);

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(object id, CancellationToken cancellationToken = default)
        {
            var entity = await session.GetAsync<T>(id, cancellationToken);
            if (entity == null)
            {
                throw new DataNotFoundException<T>(id);
            }
            return entity;
        }

        /// <inheritdoc/>
        public T Load<T>(object id) => session.Load<T>(id);

        /// <inheritdoc/>
        public IQueryable<T> Query<T>() => session.Query<T>();

        /// <inheritdoc/>
        public T Find<T>(Expression<Func<T, bool>> predicate) => session.Query<T>().SingleOrDefault(predicate);

        /// <inheritdoc/>
        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
            => await session.Query<T>().SingleOrDefaultAsync(predicate, cancellationToken);

        /// <inheritdoc/>
        public void Insert<T>(T obj) => session.Save(obj);

        /// <inheritdoc/>
        public async Task InsertAsync<T>(T obj, CancellationToken cancellationToken = default) => await session.SaveAsync(obj, cancellationToken);

        /// <inheritdoc/>
        public void Update<T>(T obj) => session.Update(obj);

        /// <inheritdoc/>
        public async Task UpdateAsync<T>(T obj, CancellationToken cancellationToken = default) => await session.UpdateAsync(obj, cancellationToken);

        /// <inheritdoc/>
        public void Delete<T>(T obj) => session.Delete(obj);

        /// <inheritdoc/>
        public async Task DeleteAsync<T>(T obj, CancellationToken cancellationToken = default) => await session.DeleteAsync(obj, cancellationToken);

        /// <inheritdoc/>
        public void Dispose() => session.Dispose();

        public async Task TransactAsync(Func<Task> asyncOperation, CancellationToken cancellationToken = default)
        {
            var currentTransaction = session.GetCurrentTransaction();
            if (currentTransaction == null || !currentTransaction.IsActive || !currentTransaction.WasCommitted || !currentTransaction.WasRolledBack)
            {
                using var tx = session.BeginTransaction();
                try
                {
                    await asyncOperation();
                    await tx.CommitAsync(cancellationToken);
                }
                catch
                {
                    await tx.RollbackAsync(cancellationToken);
                    session.Clear();
                    throw;
                }
            }
            await asyncOperation();
        }
    }
}
