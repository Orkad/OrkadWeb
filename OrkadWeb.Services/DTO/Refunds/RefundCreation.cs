using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Services.DTO.Refunds
{
    /// <summary>
    /// Correspond a une commande de création de remboursement
    /// </summary>
    public class RefundCreation
    {
        /// <summary>
        /// Destinataire du remboursement
        /// </summary>
        public int Receiver { get; set; }

        /// <summary>
        /// Montant du remboursement
        /// </summary>
        public decimal Amount { get; set; }
    }
}
