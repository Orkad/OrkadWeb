using FluentNHibernate.Mapping;
using OrkadWeb.Models;

namespace OrkadWeb.Services.NHibernate.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("user");
            Id(x => x.Id, "id");
            Map(x => x.Username).Column("username");
            Map(x => x.Password).Column("password");
            Map(x => x.Email).Column("email");
            HasMany(x => x.UserShares).KeyColumn("user_id");
        }
    }
}
