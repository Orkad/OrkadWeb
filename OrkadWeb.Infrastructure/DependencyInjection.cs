using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Infrastructure.Behaviors;
using OrkadWeb.Infrastructure.Extensions;
using OrkadWeb.Infrastructure.Jobs;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Persistence.Conventions;
using OrkadWeb.Infrastructure.Persistence.Listeners;
using OrkadWeb.Infrastructure.Services;
using System;
using System.Net;
using System.Reflection;

namespace OrkadWeb.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddNHibernate(configuration);
            var asm = Assembly.GetExecutingAssembly();
            var connectionString = configuration.GetRequiredValue("ConnectionStrings:OrkadWeb");
            var databaseType = configuration.GetRequiredValue("OrkadWeb:DatabaseType");
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
                    .ScanIn(asm).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole());


            services.AddSmtpEmailService(configuration);
            services.AddScoped<IJobRunner, JobRunner>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }

        private static IServiceCollection AddSmtpEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            services.AddSingleton<SmtpConfig>();
            return services.AddSingleton<IEmailService, SmtpEmailService>();
        }

        private static IServiceCollection AddNHibernate(this IServiceCollection services, IConfiguration configuration)
        {
            NHibernate.Cfg.Environment.ObjectsFactory = new DependencyInjectionObjectFactory(services.BuildServiceProvider());
            var connectionString = configuration.GetRequiredValue("ConnectionStrings:OrkadWeb");
            var databaseType = configuration.GetRequiredValue("OrkadWeb:DatabaseType");
            var fluentConfig = Fluently.Configure();
            fluentConfig = databaseType switch
            {
                "mysql" or "mariadb" => fluentConfig.Database(MySQLConfiguration.Standard.ConnectionString(connectionString)),
                "sqlite" => fluentConfig.Database(SQLiteConfiguration.Standard.ConnectionString(connectionString)),
                _ => throw new NotImplementedException(),
            };
            var nhConfig = fluentConfig
                .Mappings(m => m
                    .FluentMappings.AddFromAssemblyOf<OrkadWebInfrastructure>()
                    .Conventions.Add<EnumConvention>())
                .BuildConfiguration();
            services.AddScoped<OwnableListener>();
            nhConfig.AppendListeners(NHibernate.Event.ListenerType.PreUpdate, new[] { new OwnableListener() });
            services.AddSingleton(nhConfig);
            var sessionFactory = nhConfig.BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddScoped(sp => sessionFactory.OpenSession());
            services.AddScoped(sp => sessionFactory.OpenStatelessSession());
            services.AddScoped<IDataService, NHibernateDataService>();
            return services;
        }
    }
}

