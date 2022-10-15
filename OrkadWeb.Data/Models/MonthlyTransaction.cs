using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrkadWeb.Data.Models
{
    /// <summary>
    /// Monthly charge or income
    /// </summary>
    public class MonthlyTransaction : IOwnable
    {
        /// <summary>
        /// Unique identifier of the monthly charge
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Amount (positive for income, negative for charge)
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Given name of the monthly income
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Owner of the monthly transaction
        /// </summary>
        public virtual User Owner { get; set; }

        public virtual bool IsCharge() => Amount > 0;
        public virtual bool IsIncome() => Amount < 0;
    }

    public class MonthlyTransactionMap : ClassMap<MonthlyTransaction>
    {
        public MonthlyTransactionMap()
        {
            Table("monthly_transaction");
            Id(x => x.Id, "id");
            Map(x => x.Amount, "amount").Not.Nullable();
            Map(x => x.Name, "name").Length(255).Not.Nullable();
            References(x => x.Owner, "owner").Not.Nullable();
        }
    }
}
