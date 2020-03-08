using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using OrkadWeb.Models;
using System;

namespace OrkadWeb.Services
{
    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.Id, x =>
            {
                x.Column("id");
                x.Generator(Generators.Assigned);
            });

            Property(x => x.Username, x =>
            {
                x.Column("username");
            });

            Property(x => x.Password, x =>
            {
                x.Column("password");
            });

            Property(x => x.Email, x =>
            {
                x.Column("email");
            });

            
        }
    }
}
