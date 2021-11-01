using System.Linq;

namespace OrkadWeb.Data
{
    /// <summary>
    /// Lis et manipule des données (entités)
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Récupération d'une l'entité.
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        /// <exception cref="DataNotFoundException{T}">Si l'entité n'existe pas</exception>
        T Get<T>(object id);

        /// <summary>
        /// Charge l'entité sans faire d'appel en base de donnée (en assumant que l'entité existe déjà)
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        T Load<T>(object id);

        /// <summary>
        /// Création de requète sur les entités du type fourni
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        IQueryable<T> Query<T>();

        /// <summary>
        /// Ajoute une nouvelle entité
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a ajouter</param>
        void Insert<T>(T data);

        /// <summary>
        /// Met à jour une entité existante
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a mettre à jour</param>
        void Update<T>(T data);

        /// <summary>
        /// Supprime une entité existante
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a supprimer</param>
        void Delete<T>(T data);
    }
}