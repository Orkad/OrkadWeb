using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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
using OrkadWeb.Specs.Contexts;
using OrkadWeb.Specs.Drivers;
using OrkadWeb.Specs.Hooks;
using SolidToken.SpecFlow.DependencyInjection;

namespace OrkadWeb.Specs
{
    public static class Startup
    {
        [ScenarioDependencies]
        public static IServiceCollection BuildServiceCollection()
        {
            var builder = WebApplication.CreateBuilder();
            var services = builder.Services;
            services.AddNHibernate(builder.Configuration);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionCatchPipeline<,>));
            services.AddTestMigrations(builder.Configuration.GetConnectionString("OrkadWeb"));
            services.AddSingleton<JwtConfig>();
            services.AddScoped<IIdentityTokenGenerator, JwtTokenGenerator>();
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
    }
}
