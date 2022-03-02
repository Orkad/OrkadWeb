using FluentNHibernate.Mapping;
using System;

namespace OrkadWeb.Data.Models
{
    /// <summary>
    /// Représente une dépense
    /// </summary>
    public class Expense
    {
        /// <summary>
        /// Identifiant unique de la dépense
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Propriétaire de la dépense
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// Montant de la dépense
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Nom d'affichage de la dépense (identification par l'utilisateur)
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Date de la dépense
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// Partage utilisateur concerné
        /// </summary>
        public virtual UserShare UserShare { get; set; }
    }

    public class ExpenseMap : ClassMap<Expense>
    {
        public ExpenseMap()
        {
            Table("expense");
            Id(x => x.Id, "id");
            References(x => x.Owner, "owner").Not.Nullable();
            Map(x => x.Amount, "amount");
            Map(x => x.Name, "name");
            Map(x => x.Date, "date");
            References(x => x.UserShare, "user_share_id");
        }
    }
}
