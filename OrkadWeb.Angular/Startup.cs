using FluentMigrator.Runner;
using FluentNHibernate.Cfg.Db;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using OrkadWeb.Angular.Config;
using OrkadWeb.Angular.Models;
using OrkadWeb.Application;
using OrkadWeb.Application.Users;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Persistence;
using OrkadWeb.Infrastructure.Services;
using System;
using System.Net;
using System.Security.Claims;

namespace OrkadWeb.Angular
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthentication(services);

            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration, "mysql");
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/client-app";
            });

            ConfigureHangfire(services);
        }

        private void ConfigureHangfire(IServiceCollection services)
        {
            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(GetRequiredConfigValue("ConnectionStrings:Hangfire"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();
        }

        private string GetRequiredConfigValue(string key) => Configuration.GetRequiredSection(key).Value;

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddSingleton<JwtConfig>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            services.ConfigureOptions<JwtConfig>();
            services.AddSingleton<IIdentityTokenGenerator, JwtTokenGenerator>();
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUser>(ResolveAuthenticatedUser);
        }


        private AuthenticatedUser ResolveAuthenticatedUser(IServiceProvider serviceProvider)
        {
            var user = serviceProvider.GetService<IHttpContextAccessor>().HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                return new AuthenticatedUser
                {
                    Id = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value),
                    Name = user.FindFirst(ClaimTypes.Name).Value,
                    Email = user.FindFirst(ClaimTypes.Email).Value,
                };
            }
            return null;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHangfireDashboard(); // réserve la route "/hangfire"
            });
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            // automatic migration running
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetService<IMigrationRunner>().MigrateUp();
            }
        }
    }
}
