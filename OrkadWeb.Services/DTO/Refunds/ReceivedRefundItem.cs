using System;

namespace OrkadWeb.Services.DTO.Refunds
{
    /// <summary>
    /// Remboursement reçu par un utilisateur
    /// </summary>
    public class ReceivedRefundItem
    {
        /// <summary>
        /// identifiant du remboursement
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// identifiant de l'utilisateur emeteur du remboursement
        /// </summary>
        public int EmitterId { get; set; }

        /// <summary>
        /// Montant perçut
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date du remboursement
        /// </summary>
        public DateTime Date { get; set; }
    }
}
