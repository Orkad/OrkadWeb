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
            var sessionFactory = Fluently
                .Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<User>())
                .BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<ISupervisionService, SupervisionService>();
            services.AddScoped<ShareService>();
            return services;
        }
    }
}
