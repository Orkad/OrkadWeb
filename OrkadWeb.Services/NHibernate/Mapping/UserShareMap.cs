using FluentNHibernate.Mapping;
using OrkadWeb.Models;

namespace OrkadWeb.Services.NHibernate.Mapping
{
    public class UserShareMap : ClassMap<UserShare>
    {
        public UserShareMap()
        {
            Table("user_share");
            Id(x => x.Id, "id");
            References(x => x.User, "user_id");
            References(x => x.Share, "share_id");
            HasMany(x => x.Expenses).KeyColumn("user_share_id").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.EmittedRefunds).KeyColumn("user_share_emitter_id");
            HasMany(x => x.ReceivedRefunds).KeyColumn("user_share_receiver_id");
        }
    }
}
