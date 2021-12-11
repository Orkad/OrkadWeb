using NHibernate;
using OrkadWeb.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Data
{
    internal class DataService : IDisposable, IDataService
    {
        private readonly ISession session;

        public DataService(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Récupération d'une l'entité.
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        /// <exception cref="DataNotFoundException{T}">Si l'entité n'existe pas</exception>
        public T Get<T>(object id) => session.Get<T>(id) ?? throw new DataNotFoundException<T>(id);

        /// <summary>
        /// Récupération d'une l'entité (asynchrone)
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        /// <exception cref="DataNotFoundException{T}">Si l'entité n'existe pas</exception>
        public async Task<T> GetAsync<T>(object id) => await session.GetAsync<T>(id) ?? throw new DataNotFoundException<T>(id);

        /// <summary>
        /// Charge l'entité sans faire d'appel en base de donnée (en assumant que l'entité existe déjà)
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        public T Load<T>(object id) => session.Load<T>(id);

        /// <summary>
        /// Création de requète sur les entités du type fourni
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        public IQueryable<T> Query<T>() => session.Query<T>();

        public void Insert<T>(T obj)
        {
            CreateTransactionIfNotExists();
            session.Save(obj);
        }

        public async Task InsertAsync<T>(T obj)
        {
            CreateTransactionIfNotExists();
            await session.SaveAsync(obj);
        }

        public void Update<T>(T obj)
        {
            CreateTransactionIfNotExists();
            session.Update(obj);
        }

        public async Task UpdateAsync<T>(T obj)
        {
            CreateTransactionIfNotExists();
            await session.UpdateAsync(obj);
        }

        /// <summary>
        /// Supprime une entité existante
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="obj">instance de l'entité a supprimer</param>
        public void Delete<T>(T obj)
        {
            CreateTransactionIfNotExists();
            session.Delete(obj);
        }

        /// <summary>
        /// Créé une transaction si aucun n'est existante
        /// </summary>
        private void CreateTransactionIfNotExists()
        {
            if (session.GetCurrentTransaction() == null)
            {
                session.BeginTransaction();
            }
        }

        public void Dispose()
        {
            session.GetCurrentTransaction()?.Commit();
            session.Dispose();
        }
    }
}
