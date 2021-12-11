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
            var nhConfigCache = new NHibernateConfigurationCache(assembly);
            var config = nhConfigCache.LoadConfigurationFromFile();
            if (config == null)
            {
                config = Fluently.Configure()
                    .Database(MySQLConfiguration.Standard)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(assembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
                nhConfigCache.SaveConfigurationToFile(config);
            }
            // set connection string after (we don't want to keep it in cache file)
            config.SetProperty("connection.connection_string", connectionString);

            // Run database creation
            new SchemaUpdate(config).Execute(false, true);

            return config.BuildSessionFactory();
        }
    }
}
