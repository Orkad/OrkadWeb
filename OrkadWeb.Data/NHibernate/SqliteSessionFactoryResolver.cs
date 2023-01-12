using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System.Reflection;

namespace OrkadWeb.Data.NHibernate
{
    public class SqliteSessionFactoryResolver : ISessionFactoryResolver
    {
        private readonly string connectionString;

        public SqliteSessionFactoryResolver(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public ISessionFactory Resolve(Assembly assembly)
        {
            var config = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(assembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();

            return config.BuildSessionFactory();
        }
    }
}
