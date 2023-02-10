using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;

namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// Représente un utilisateur de "OrkadWeb"
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Mot de passe de l'utilisateur (crypté)
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Adresse email de contact de l'utilisateur
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Creation date of the user account
        /// </summary>
        public virtual DateTime Creation { get; set; }

        /// <summary>
        /// User confirmation date (email confirmation)
        /// </summary>
        public virtual DateTime? Confirmation { get; set; }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("user");
            Id(x => x.Id, "id");
            Map(x => x.Username).Column("username").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.Creation).Column("creation").Not.Nullable();
            Map(x => x.Confirmation).Column("confirmation").Nullable();
        }
    }
}
