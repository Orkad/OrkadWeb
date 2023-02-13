using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Application;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Tests.Contexts;
using SolidToken.SpecFlow.DependencyInjection;

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
            services.AddInfrastructureServices("sqlite", connectionString);
            services.AddApplicationServices();
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
