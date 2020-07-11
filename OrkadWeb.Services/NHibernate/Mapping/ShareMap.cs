using FluentNHibernate.Mapping;
using OrkadWeb.Models;
using OrkadWeb.Models.Enums;

namespace OrkadWeb.Services.NHibernate.Mapping
{
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
