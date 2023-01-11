using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace OrkadWeb.Data.NHibernate
{
    public class MySQLSessionFactoryResolver : ISessionFactoryResolver
    {
        private readonly string connectionString;

        public MySQLSessionFactoryResolver(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ISessionFactory Resolve(Assembly assembly)
        {
            var config = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(assembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
            return config.BuildSessionFactory();
        }
    }
}
