using System.Collections.Generic;

namespace OrkadWeb.Models
{
    /// <summary>
    /// Représente un partage utilisateur
    /// </summary>
    public class UserShare
    {
        /// <summary>
        /// Identifiant unique de l'association
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Référence vers l'utilisateur
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Référence vers le partage
        /// </summary>
        public virtual Share Share { get; set; }

        /// <summary>
        /// Intégralité des dépenses sur ce partage utilisateur
        /// </summary>
        public virtual ISet<Expense> Expenses { get; set; }

        /// <summary>
        /// Remboursement emis depuis ce partage utilisateur
        /// </summary>
        public virtual ISet<Refund> EmittedRefunds { get; set; }

        /// <summary>
        /// Remboursement reçu depuis ce partage utilisateur
        /// </summary>
        public virtual ISet<Refund> ReceivedRefunds { get; set; }
    }
}
