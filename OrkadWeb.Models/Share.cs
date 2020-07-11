using OrkadWeb.Models.Enums;
using System.Collections.Generic;

namespace OrkadWeb.Models
{
    /// <summary>
    /// Représente un "partage" sur OrkadWeb
    /// </summary>
    public class Share
    {
        /// <summary>
        /// Identifiant unique du partage
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Nom d'affichage du partage
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Propriétaire du partage
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// Associations des utilisateurs
        /// </summary>
        public virtual ISet<UserShare> UserShares { get; set; }

        /// <summary>
        /// Règles concernant la modification du partage par les utilisateurs
        /// </summary>
        public virtual ShareRule Rule { get; set; }
    }
}
