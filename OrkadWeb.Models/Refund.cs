using System;

namespace OrkadWeb.Models
{
    /// <summary>
    /// Représente un remboursement
    /// </summary>
    public class Refund
    {
        /// <summary>
        /// Identifiant unique du remboursement
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Montant du remboursement
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Emmeteur du remboursement
        /// </summary>
        public virtual UserShare Emitter { get; set; }

        /// <summary>
        /// Destinataire du remboursement
        /// </summary>
        public virtual UserShare Receiver { get; set; }

        /// <summary>
        /// Date effective du remboursement
        /// </summary>
        public virtual DateTime Date { get; set; }
    }
}
