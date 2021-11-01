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
            var nhConfigCache = new NHibernateConfigurationCache(mappingAssembly);
            var config = nhConfigCache.LoadConfigurationFromFile();
            if (config == null)
            {
                config = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
                nhConfigCache.SaveConfigurationToFile(config);
            }
            // set connection string after (we don't want to keep it in cache file)
            config.SetProperty("connection.connection_string", connectionString);

            // Run database creation
            new SchemaExport(config).Execute(false, true, false);

            return config.BuildSessionFactory();
        }
    }
}
