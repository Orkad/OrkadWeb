using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Services.DTO.Refunds
{
    /// <summary>
    /// plus petite représentation d'un remboursement
    /// </summary>
    public class RefundItem
    {
        /// <summary>
        /// identifiant de l'entité de remboursement
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// identifiant de l'emmeteur du remboursement
        /// </summary>
        public int EmitterId { get; set; }

        /// <summary>
        /// identifiant du destinataire du remboursement
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        /// montant du remboursement
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date du remboursement
        /// </summary>
        public DateTime Date { get; set; }
    }
}
