using System;

namespace OrkadWeb.Models
{
    /// <summary>
    /// Représente un utilisateur de "OrkadWeb"
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifiant unique de l'utilisateur
        /// </summary>
        public virtual uint Id { get; set; }

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
    }

    // CREATE TABLE users(id INT PRIMARY KEY AUTO_INCREMENT, username VARCHAR(255), password VARCHAR(255), email VARCHAR(255));
}
