using OrkadWeb.Services.DTO.Expenses;
using OrkadWeb.Services.DTO.Operations;
using System.Collections.Generic;

namespace OrkadWeb.Services.DTO.Shares
{
    public class UserShareDetail
    {
        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dépense de l'utilisateur sur le partage
        /// </summary>
        public List<ExpenseItem> Expenses { get; set; } = new List<ExpenseItem>();

        /// <summary>
        /// Liste des opérations (dépenses, remboursements émits et reçus)
        /// </summary>
        public List<OperationItem> Operations { get; set; } = new List<OperationItem>();
    }
}
