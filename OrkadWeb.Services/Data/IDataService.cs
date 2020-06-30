using System.Linq;

namespace OrkadWeb.Services.Data
{
    public interface IDataService
    {
        /// <summary>
        /// Récupère un <see cref="IQueryable{T}"/> de <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQueryable<T> Query<T>();

        /// <summary>
        /// Récupère un <typeparamref name="T"/> par son identifiant unique.
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        T Get<T>(object id);

        /// <summary>
        /// Charge une entité sans controler son existence
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        T Load<T>(object id);

        void Insert<T>(T obj);
        void Update<T>(T obj);
        void Delete<T>(T obj);
    }
}