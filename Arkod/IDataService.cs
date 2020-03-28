using System.Linq;

namespace OrkadWeb.Services
{
    /// <summary>
    /// Représente une façon d'accéder à un modèle de données
    /// </summary>
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
        /// Si le <typeparamref name="T"/> n'existe pas, lance une 
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        T Get<T>(object id);
    }
}