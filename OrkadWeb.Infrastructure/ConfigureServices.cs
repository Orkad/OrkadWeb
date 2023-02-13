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
using FluentNHibernate.Cfg;
using OrkadWeb.Infrastructure.Persistence.Conventions;

namespace OrkadWeb.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetRequiredValue("ConnectionStrings:OrkadWeb");
            var databaseType = configuration.GetRequiredValue("OrkadWeb:DatabaseType");
            var fluentConfig = Fluently.Configure();
            services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                {
                    switch (databaseType)
                    {
                        case "mysql":
                        case "mariadb":
                            builder.AddMySql5();
                            fluentConfig = fluentConfig.Database(MySQLConfiguration.Standard.ConnectionString(connectionString));
                            break;
                        case "sqlite":
                            builder.AddSQLite();
                            fluentConfig = fluentConfig.Database(SQLiteConfiguration.Standard.ConnectionString(connectionString));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    builder
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            fluentConfig = fluentConfig.Mappings(m => m
                        .FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                        .Conventions.Add<EnumConvention>());
            var nhConfiguration = fluentConfig.BuildConfiguration();
            var sessionFactory = nhConfiguration.BuildSessionFactory();
            services.AddSingleton(nhConfiguration);
            services.AddSingleton(sessionFactory);
            services.AddScoped(sp => sp.GetService<ISessionFactory>().OpenSession());
            services.AddScoped(sp => sp.GetService<ISessionFactory>().OpenStatelessSession());
            services.AddScoped<IDataService, DataService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IEmailService, SmtpEmailService>();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            return services;
        }
    }
}

