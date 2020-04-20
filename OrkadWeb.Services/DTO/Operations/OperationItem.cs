using System;

namespace OrkadWeb.Services.DTO.Operations
{
    /// <summary>
    /// une opération représente une dépense ou un remboursement emit ou reçu sur un partage
    /// </summary>
    public class OperationItem
    {
        /// <summary>
        /// Nom d'affichage
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Montant de l'opération (+ ou -)
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Date de l'opération
        /// </summary>
        public DateTime Date { get; set; }
    }
}
