using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using OrkadWeb.Angular.Config;
using OrkadWeb.Angular.Controllers;
using OrkadWeb.Application;
using OrkadWeb.Application.Users;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Persistence.Conventions;
using OrkadWeb.Tests.Drivers;
using OrkadWeb.Tests.Hooks;
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
            services.AddScoped<AuthController>();
            services.AddScoped<IAppUser>(sp => sp.GetRequiredService<UserContext>().AuthenticatedUser);
            services.AddHttpContextAccessor();
            return services;
        }

        public static IServiceCollection AddTestInfrastructureServices(this IServiceCollection services)
        {
            var connectionString = "FullUri=file:memorydb.db?mode=memory&cache=shared";

            services.AddTestMigrations(connectionString);

            services.AddSingleton(Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                    .Mappings(m => m
                    .FluentMappings.AddFromAssembly(OrkadWebInfrastructure.Assembly)
                    .Conventions.Add<EnumConvention>())
                    .ExposeConfiguration(c =>
                    {
                        c.SetProperty("hbm2ddl.keywords", "auto-quote");
                    })
                    .BuildConfiguration())
                .AddSingleton(sp => sp.GetRequiredService<Configuration>().BuildSessionFactory())
                .AddScoped(sp => sp.GetRequiredService<ISessionFactory>().OpenSession())
                .AddScoped(sp => sp.GetRequiredService<ISessionFactory>().OpenStatelessSession())
                .AddScoped<IDataService, NHibernateDataService>()
                .AddScoped<IDataContext, DataContext>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionCatchPipeline<,>));

            services.AddSingleton<JwtConfig>();
            services.AddScoped<IIdentityTokenGenerator, JwtTokenGenerator>();

            return services;
        }
    }
}
