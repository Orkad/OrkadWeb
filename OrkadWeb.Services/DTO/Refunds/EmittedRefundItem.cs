using System;

namespace OrkadWeb.Services.DTO.Refunds
{
    /// <summary>
    /// Remboursement emit par un utilisateur
    /// </summary>
    public class EmittedRefundItem
    {
        /// <summary>
        /// identifiant du remboursement
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// identifiant de l'utilisateur du remboursement
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// Montant donné
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date du remboursement
        /// </summary>
        public DateTime Date { get; set; }
    }
}
