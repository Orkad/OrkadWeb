using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Domain.Extensions;
using OrkadWeb.Infrastructure.Jobs;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Persistence.Conventions;
using OrkadWeb.Infrastructure.Services;
using System;
using System.Net;
using System.Reflection;

namespace OrkadWeb.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetRequiredValue("ConnectionStrings:OrkadWeb");
            var databaseType = configuration.GetRequiredValue("OrkadWeb:DatabaseType");
            var fluentConfig = Fluently.Configure();
            fluentConfig = databaseType switch
            {
                "mysql" or "mariadb" => fluentConfig.Database(MySQLConfiguration.Standard.ConnectionString(connectionString)),
                "sqlite" => fluentConfig.Database(SQLiteConfiguration.Standard.ConnectionString(connectionString)),
                _ => throw new NotImplementedException(),
            };
            return services.AddFluentMigratorCore()
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
                .AddLogging(lb => lb.AddFluentMigratorConsole())

            .AddSingleton(sp => fluentConfig
                .Mappings(m => m
                    .FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add<EnumConvention>())
                .BuildConfiguration())

            .AddSingleton(sp => sp.GetService<Configuration>().BuildSessionFactory())
            .AddScoped(sp => sp.GetService<ISessionFactory>().OpenSession())
            .AddScoped(sp => sp.GetService<ISessionFactory>().OpenStatelessSession())
            .AddScoped<IRepository, NHibernateRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddSmtpEmailService(configuration)
            .AddScoped<IJobRunner, JobRunner>();
        }

        private static IServiceCollection AddSmtpEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            services.AddSingleton<SmtpConfig>();
            return services.AddSingleton<IEmailService, SmtpEmailService>();
        }
    }
}

