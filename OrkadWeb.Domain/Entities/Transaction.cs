﻿using System;
using OrkadWeb.Domain.Primitives;

namespace OrkadWeb.Domain.Entities
{
    /// <summary>
    /// A money loss of a user. Amount can be negative for incomes.
    /// </summary>
    public class Transaction : IOwnable
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
}
