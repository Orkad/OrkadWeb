using FluentMigrator.Runner;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using OrkadWeb.Angular.Config;
using OrkadWeb.Application;
using OrkadWeb.Application.Common.Interfaces;
using OrkadWeb.Application.Users;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Persistence.Conventions;
using OrkadWeb.Tests.Contexts;
using OrkadWeb.Tests.Drivers;
using SolidToken.SpecFlow.DependencyInjection;

namespace OrkadWeb.Tests
{
    public static class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection BuildServiceCollection()
        {
            var builder = WebApplication.CreateBuilder();
            var services = builder.Services;
            services.AddTestInfrastructureServices();
            services.AddScoped<IEmailService, EmailTestService>();

            services.AddApplicationServices();
            var timeContext = new TimeContext();
            services.AddSingleton(timeContext);
            services.AddSingleton<ITimeProvider>(timeContext);
            return services;
        }

        public static IServiceCollection AddTestInfrastructureServices(this IServiceCollection services)
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
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionCatchPipeline<,>));

            services.AddSingleton<JwtConfig>();
            services.AddScoped<IIdentityTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
