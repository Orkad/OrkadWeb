using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Models.Enums.NHibernate;
using OrkadWeb.Services.NHibernate.Mapping;
using System;
using System.Reflection;
using System.Threading;

namespace OrkadWeb.Services.NHibernate
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, string connectionString)
        {
            // Assembly that contains mapping
            var mappingAssembly = Assembly.Load("OrkadWeb.Services");
            var nhConfigCache = new NHibernateConfigurationCache(mappingAssembly);
            var config = nhConfigCache.LoadConfigurationFromFile(); // 175ms (si cache présent)
            if (config == null)
            {
                var fluentConfiguration = Fluently
                    .Configure()
                    .Database(MySQLConfiguration.Standard)
                    .Mappings(m => m.FluentMappings.AddFromAssembly(mappingAssembly).Conventions.Add<EnumConvention>()); // 132ms
                config = fluentConfiguration.BuildConfiguration(); // 549ms
                nhConfigCache.SaveConfigurationToFile(config); // 60ms
            }
            config.SetProperty("connection.connection_string", connectionString);
            var sessionFactory = config.BuildSessionFactory(); // 539ms 

            services.AddSingleton(sessionFactory);
            services.AddScoped(serviceProvider => sessionFactory.OpenSession());
            return services;
        }
    }
}
