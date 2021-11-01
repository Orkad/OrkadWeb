using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OrkadWeb.Data.Builder
{
    public static class DataAppBuilderExtensions
    {
        public static void AddData(this IServiceCollection services, string connectionString)
        {
            var sessionFactory = SessionFactoryResolver.Resolve(connectionString);
            // session factory always as singleton
            services.AddSingleton(sessionFactory);
            // session always scoped
            services.AddScoped(serviceProvider => sessionFactory.OpenSession());

            // dataservice always scoped
            services.AddScoped<IDataService, DataService>();
        }
    }

}
