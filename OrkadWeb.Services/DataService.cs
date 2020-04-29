using NHibernate;
using OrkadWeb.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrkadWeb.Services
{
    public class DataService : IDataService, IDisposable
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
        /// <exception cref="NotFoundException{T}">Si l'entité n'existe pas</exception>
        public T Get<T>(object id) => session.Get<T>(id) ?? throw new NotFoundException<T>(id);

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

        public void Update<T>(T obj)
        {
            CreateTransactionIfNotExists();
            session.Update(obj);
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
            if (!session.Transaction.IsActive)
            {
                session.BeginTransaction();
            }
        }

        public void Dispose()
        {
            if (session.Transaction.IsActive)
            {
                session.Transaction.Commit();
            }
        }
    }
}
