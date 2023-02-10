using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using OrkadWeb.Domain.NHibernate;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OrkadWeb.Domain.Builder
{
    public static class DataAppBuilderExtensions
    {
        public static void AddData(this IServiceCollection services)
        {
            // Session
            services.AddScoped(provider => provider.GetService<ISessionFactory>().OpenSession());
            services.AddScoped(provider => provider.GetService<ISessionFactory>().OpenStatelessSession());

            // DataService
            services.AddScoped<IDataService, DataService>();
        }
    }

}
