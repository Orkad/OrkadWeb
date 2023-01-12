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
            var nhConfigCache = new NHibernateConfigurationCache(assembly);
            var config = nhConfigCache.LoadConfigurationFromFile();
            if (config == null)
            {
                config = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(assembly)
                        .Conventions.Add<EnumConvention>())
                    .BuildConfiguration();
                nhConfigCache.SaveConfigurationToFile(config);
            }
            // set connection string after (we don't want to keep it in cache file)
            config.SetProperty("connection.connection_string", connectionString);

            return config.BuildSessionFactory();
        }
    }
}
