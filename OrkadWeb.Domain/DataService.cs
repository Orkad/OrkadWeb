using NHibernate;
using OrkadWeb.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrkadWeb.Domain
{
    /// <inheritdoc cref="IDataService"/>
    internal class DataService : IDisposable, IDataService
    {
        private readonly ISession session;

        public DataService(ISession session)
        {
            this.session = session;
        }

        /// <inheritdoc/>
        public T Get<T>(object id) => session.Get<T>(id) ?? throw new DataNotFoundException<T>(id);

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(object id, CancellationToken cancellationToken = default) => await session.GetAsync<T>(id, cancellationToken) ?? throw new DataNotFoundException<T>(id);

        /// <inheritdoc/>
        public T Load<T>(object id) => session.Load<T>(id);

        /// <inheritdoc/>
        public IQueryable<T> Query<T>() => session.Query<T>();

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
    }
}
