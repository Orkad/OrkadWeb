using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.Attributes;
using NHibernate.Mapping.ByCode;
using OrkadWeb.Models;

namespace OrkadWeb.Services
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {

            var sessionFactory = CreateSessionFactory(connectionString);
            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<ShareService>();
            return services;
        }

        private static ISessionFactory CreateSessionFactory(string connectionString)
        {
            return Fluently
                .Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>())
                .BuildSessionFactory();
        }

        private static ISessionFactory CreateSessionFactoryOld(string connectionString)
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MySQLDialect>();
                c.Driver<MySqlDataDriver>();
                c.ConnectionString = connectionString;
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                c.SchemaAction = SchemaAutoAction.Validate;
                c.LogFormattedSql = true;
                c.LogSqlInConsole = true;
            });

            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(User).Assembly.ExportedTypes);
            HbmMapping domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddMapping(domainMapping);
            return configuration.BuildSessionFactory();
        }
    }
}
