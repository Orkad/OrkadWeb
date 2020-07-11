using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Models.Enums.NHibernate;
using OrkadWeb.Services.NHibernate.Mapping;

namespace OrkadWeb.Services.NHibernate
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            var sessionFactory = Fluently
                .Configure()
                .Database(MySQLConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>().Conventions.Add<EnumConvention>())
                .BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddScoped(factory => sessionFactory.OpenSession());
            return services;
        }
    }
}
