using System;

namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// User of the OrkadWeb application
    /// </summary>
    public class User
    {
        /// <summary>
        /// User unique identifier
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Encrypted user password
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Email address of the user
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

        /// <summary>
        /// Unique role of the user
        /// </summary>
        public virtual string Role { get; set; }
    }
}
