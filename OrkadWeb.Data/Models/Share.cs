using FluentNHibernate.Mapping;
using OrkadWeb.Data.Models.Enums;
using System.Collections.Generic;

namespace OrkadWeb.Data.Models
{
    /// <summary>
    /// Représente un "partage" sur OrkadWeb
    /// </summary>
    public class Share
    {
        /// <summary>
        /// Identifiant unique du partage
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// Nom d'affichage du partage
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Propriétaire du partage
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// Associations des utilisateurs
        /// </summary>
        public virtual ISet<UserShare> UserShares { get; set; }

        /// <summary>
        /// Règles concernant la modification du partage par les utilisateurs
        /// </summary>
        public virtual ShareRule Rule { get; set; }
    }

    public class ShareMap : ClassMap<Share>
    {
        public ShareMap()
        {
            Table("share");
            Id(x => x.Id, "id");
            Map(x => x.Name, "name");
            References(x => x.Owner, "owner").Cascade.None();
            HasMany(x => x.UserShares).KeyColumn("share_id").Inverse().Cascade.AllDeleteOrphan();
            Map(x => x.Rule, "rule").CustomType<ShareRule>();
        }
    }
}
