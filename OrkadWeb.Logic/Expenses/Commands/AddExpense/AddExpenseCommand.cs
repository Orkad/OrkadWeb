﻿using OrkadWeb.Logic.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Logic.Expenses.Commands.AddExpense
{
    /// <summary>
    /// Permet d'ajouter une dépense
    /// </summary>
    public class AddExpenseCommand : ICommand<AddExpenseResult>
    {
        /// <summary>
        /// Montant de la dépense
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Nom d'affichage de la dépense
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date de la dépense (optionnel)
        /// </summary>
        public DateTime? Date { get; set; }
    }
}