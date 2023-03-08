using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using OrkadWeb.Application;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Persistence.Conventions;
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
            services.AddTestInfrastructureServices(configuration);
            services.AddScoped<IEmailService, EmailTestService>();

            services.AddApplicationServices();
            var timeContext = new TimeContext();
            services.AddSingleton(timeContext);
            services.AddSingleton<ITimeProvider>(timeContext);

            return services;
        }

        public static IServiceCollection AddTestInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = "FullUri=file:memorydb.db?mode=memory&cache=shared";

            services.AddFluentMigratorCore()
                .ConfigureRunner(b => b
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(OrkadWebInfrastructure.Assembly))
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddSingleton(Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m
                    .FluentMappings.AddFromAssembly(OrkadWebInfrastructure.Assembly)
                    .Conventions.Add<EnumConvention>())
                    .BuildConfiguration())
                .AddSingleton(sp => sp.GetRequiredService<Configuration>().BuildSessionFactory())
                .AddScoped(sp => sp.GetRequiredService<ISessionFactory>().OpenSession())
                .AddScoped(sp => sp.GetRequiredService<ISessionFactory>().OpenStatelessSession())
                .AddScoped<IDataService, DataService>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IJobRunner>((sp) => sp.GetRequiredService<JobRunnerContext>());

            return services;
        }
    }
}
