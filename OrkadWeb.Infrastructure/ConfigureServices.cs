using FluentMigrator.Runner;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Extensions;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Services;
using System;
using System.Net;
using System.Reflection;
using MySql.Data.MySqlClient;
using NHibernate;

namespace OrkadWeb.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, string databaseType)
        {
            var connectionString = configuration.GetRequiredValue("ConnectionStrings:OrkadWeb");
            services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                {
                    switch (databaseType)
                    {
                        case "mysql":
                        case "mariadb":
                            builder.AddMySql5();
                            break;
                        case "sqlite":
                            builder.AddSQLite();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    builder
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            var mysql = MySQLConfiguration.Standard.ConnectionString(connectionString);
            var nhConfiguration = OrkadWebConfigurationBuilder.Build(mysql);
            services.AddSingleton(nhConfiguration);
            var sessionFactory = nhConfiguration.BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddScoped(sp => sp.GetService<ISessionFactory>().OpenSession());
            services.AddScoped(sp => sp.GetService<ISessionFactory>().OpenStatelessSession());
            services.AddScoped<IDataService, DataService>();

            services.AddSingleton<IEmailService, SmtpEmailService>();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            return services;
        }
    }
}

