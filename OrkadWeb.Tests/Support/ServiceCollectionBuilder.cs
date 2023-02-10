using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Domain;
using OrkadWeb.Domain.Builder;
using OrkadWeb.Domain.NHibernate;
using OrkadWeb.Domain.Migrator;
using OrkadWeb.Logic;
using OrkadWeb.Logic.Abstractions;
using OrkadWeb.Tests.Contexts;
using OrkadWeb.Tests.Models;
using SolidToken.SpecFlow.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TechTalk.SpecFlow;
using FluentMigrator.Runner;
using OrkadWeb.Common;

namespace OrkadWeb.Tests.Support
{
    public static class ServiceCollectionBuilder
    {
        [ScenarioDependencies]
        public static IServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();

            var connectionString = "FullUri=file:memorydb.db?mode=memory&cache=shared";
            var sqlite = SQLiteConfiguration.Standard.ConnectionString(connectionString);
            var configuration = OrkadWebConfigurationBuilder.Build(sqlite);
            var sessionFactory = configuration.BuildSessionFactory();
            var connection = sessionFactory.OpenSession().Connection;
            services.AddSingleton(sessionFactory);
            services.AddSingleton(connection); // for keeping in memory connection up
            services.AddOrkadWebMigrator("sqlite", connectionString);
            services.AddData();
            services.AddLogic();
            var timeContext = new TimeContext();
            services.AddSingleton(timeContext);
            services.AddSingleton<ITimeProvider>(timeContext);
            var emailContext = new EmailContext();
            services.AddSingleton(emailContext);
            services.AddSingleton<IEmailService>(emailContext);
            return services;
        }


    }
}
