﻿using FluentNHibernate.Mapping;

namespace OrkadWeb.Models
{
    /// <summary>
    /// Représente une dépense
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// Identifiant unique de l'association
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Partage utilisateur concerné
        /// </summary>
        public virtual UserShare UserShare { get; set; }

        /// <summary>
        /// Montant de la dépense
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Nom d'affichage de la dépense (identification par l'utilisateur)
        /// </summary>
        public virtual string Name { get; set; }
    }

    public class ExpenseMap : ClassMap<Expense>
    {
        public ExpenseMap()
        {
            Table("expense");
            Id(x => x.Id, "id");
            Map(x => x.Amount, "amount");
            Map(x => x.Name, "name");
            References(x => x.UserShare, "user_share_id");
        }
    }
}
