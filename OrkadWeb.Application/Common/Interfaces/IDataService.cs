using System;
using System.Linq;
using System.Linq.Expressions;

namespace OrkadWeb.Application.Common.Interfaces
{
    /// <summary>
    /// Service for manipulating persistent database data
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Get one entity based on id.
        /// </summary>
        T Get<T>(object id);
        /// <inheritdoc cref="Get{T}(object)"/>
        Task<T> GetAsync<T>(object id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Load entity assuming that is already exists.
        /// </summary>
        T Load<T>(object id);

        /// <summary>
        /// Get a Linq query of the entity to build over.
        /// </summary>
        IQueryable<T> Query<T>();

        /// <summary>
        /// Find the single matching entity.
        /// </summary>
        T Find<T>(Expression<Func<T, bool>> predicate);
        /// <inheritdoc cref="Find{T}(Expression{Func{T, bool}})"/>
        Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new entity.
        /// </summary>
        void Insert<T>(T data);
        /// <inheritdoc cref="Insert{T}(T)"/>
        Task InsertAsync<T>(T data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a existing entity.
        /// </summary>
        void Update<T>(T data);
        /// <inheritdoc cref="Insert{T}(T)"/>
        Task UpdateAsync<T>(T data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a existing entity.
        /// </summary>
        void Delete<T>(T data);
        /// <inheritdoc cref="Delete{T}(T)"/>
        Task DeleteAsync<T>(T data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Execute the operation inside a transaction.
        /// If an exception is thrown during the operation nothing is commit.
        /// </summary>
        Task TransactAsync(Func<Task> asyncOperation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a new transactionnal data context.
        /// Remember to always use <see cref="IDataContext"/> with using.
        /// If you change something in the context don't forget to call <see cref="IDataContext.SaveChanges(CancellationToken)"/>
        /// </summary>
        IDataContext Context();
    }

    /// <summary>
    /// Défini des méthodes d'extension pour un <see cref="IDataService"/>
    /// </summary>
    public static class IDataServiceExtensions
    {
        /// <summary>
        /// Détermine si au moins une entité correspond à la condition passée en paramètre
        /// </summary>
        public static bool Exists<T>(this IDataService dataService, Expression<Func<T, bool>> condition)
            => dataService.Query<T>().Any(condition);

        /// <summary>
        /// Détermine si aucune entité ne correspond à la condition passée en paramètre
        /// </summary>
        public static bool NotExists<T>(this IDataService dataService, Expression<Func<T, bool>> condition)
            => !dataService.Exists(condition);

        /// <summary>
        /// Delete an entity without retrieving it before
        /// </summary>
        public static void DeleteById<T>(this IDataService dataService, object id)
            => dataService.Delete(dataService.Load<T>(id));
    }
}
