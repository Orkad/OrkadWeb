using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace OrkadWeb.Data.NHibernate
{
    internal static class SessionFactoryResolver
    {
        public static ISessionFactory Resolve(string connectionString)
        {
            // Assembly that contains mapping
            var mappingAssembly = Assembly.GetExecutingAssembly();
            var config = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();

            // Run database creation
            new SchemaExport(config).Execute(false, true, false);

            return config.BuildSessionFactory();
        }
    }
}
