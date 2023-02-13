using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Application;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Tests.Contexts;
using SolidToken.SpecFlow.DependencyInjection;

namespace OrkadWeb.Tests
{
    public static class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddInfrastructureServices(configuration);
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
