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
            Table("USERS");
            Id(x => x.Id, x =>
            {
                x.Generator(Generators.Guid);
                x.Type(NHibernateUtil.Guid);
                x.Column("ID");
                x.UnsavedValue(Guid.Empty);
            });

            Property(x => x.Username, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
            });

            Property(x => x.Password, x =>
            {
                x.Length(200);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
            });

            Property(x => x.Email, x =>
            {
                x.Length(200);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
            });

            
        }
    }
}
