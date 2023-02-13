using FluentNHibernate.Mapping;
using OrkadWeb.Domain.Entities;

namespace OrkadWeb.Infrastructure.Persistence.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("user");
            Id(x => x.Id, "id");
            Map(x => x.Username).Column("username").Not.Nullable();
            Map(x => x.Password).Column("password").Not.Nullable();
            Map(x => x.Email).Column("email").Not.Nullable();
            Map(x => x.Creation).Column("creation").Not.Nullable();
            Map(x => x.Confirmation).Column("confirmation").Nullable();
        }
    }
}
