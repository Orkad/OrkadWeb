using FluentNHibernate.Mapping;
using System;

namespace OrkadWeb.Data.Models
{
    /// <summary>
    /// A money loss of a user. Amount can be negative for incomes.
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Id of the transaction
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Transaction Owner
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// Transaction Amount (income are negatives)
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Transaction display
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Transaction date
        /// </summary>
        public virtual DateTime Date { get; set; }

    }

    public class TransactionMap : ClassMap<Transaction>
    {
        public TransactionMap()
        {
            Table("expense");
            Id(x => x.Id, "id");
            References(x => x.Owner, "owner").Not.Nullable();
            Map(x => x.Amount, "amount");
            Map(x => x.Name, "name");
            Map(x => x.Date, "date");
        }
    }
}
