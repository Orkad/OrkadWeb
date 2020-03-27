﻿using FluentNHibernate.Mapping;

namespace OrkadWeb.Models
{
    /// <summary>
    /// Représente un remboursement
    /// </summary>
    public class Refund
    {
        /// <summary>
        /// Identifiant unique du remboursement
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Montant du remboursement
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Emmeteur du remboursement
        /// </summary>
        public virtual UserShare Emitter { get; set; }

        /// <summary>
        /// Destinataire du remboursement
        /// </summary>
        public virtual UserShare Receiver { get; set; }
    }

    public class RefundMap : ClassMap<Refund>
    {
        public RefundMap()
        {
            Table("refund");
            Id(x => x.Id, "id");
            References(x => x.Emitter, "user_share_emitter_id");
            References(x => x.Receiver, "user_share_receiver_id");
        }
    }
}
