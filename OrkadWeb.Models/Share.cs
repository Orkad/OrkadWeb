using FluentNHibernate.Mapping;
using System.Collections.Generic;

namespace OrkadWeb.Models
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
        }
    }
}
