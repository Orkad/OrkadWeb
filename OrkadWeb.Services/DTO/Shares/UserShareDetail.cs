using OrkadWeb.Services.DTO.Expenses;
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
    }
}
