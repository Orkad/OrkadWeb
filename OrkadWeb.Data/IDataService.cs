﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
        /// Récupération d'une l'entité.
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="id">identifiant unique de l'entité</param>
        /// <exception cref="DataNotFoundException{T}">Si l'entité n'existe pas</exception>
        Task<T> GetAsync<T>(object id);

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
        /// Ajoute une nouvelle entité
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a ajouter</param>
        Task InsertAsync<T>(T data);

        /// <summary>
        /// Met à jour une entité existante
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a mettre à jour</param>
        void Update<T>(T data);

        /// <summary>
        /// Met à jour une entité existante
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a mettre à jour</param>
        Task UpdateAsync<T>(T data);

        /// <summary>
        /// Supprime une entité existante
        /// </summary>
        /// <typeparam name="T">type de l'entité</typeparam>
        /// <param name="data">instance de l'entité a supprimer</param>
        void Delete<T>(T data);
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
    }
}