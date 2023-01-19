using FluentMigrator.Runner;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using OrkadWeb.Angular.Config;
using OrkadWeb.Angular.Models;
using OrkadWeb.Data;
using OrkadWeb.Data.Builder;
using OrkadWeb.Data.Migrator;
using OrkadWeb.Data.NHibernate;
using OrkadWeb.Logic;
using OrkadWeb.Logic.Users;
using System;
using System.Configuration;
using System.Reflection;
using System.Text;

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

            ConfigureDatabase(services);

            services.AddData();
            services.AddLogic();
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/client-app";
            });
        }

        private string GetRequiredConfigValue(string key) => Configuration.GetRequiredSection(key).Value;

        private void ConfigureDatabase(IServiceCollection services)
        {
            var builder = new MySqlConnectionStringBuilder(GetRequiredConfigValue("ConnectionStrings:OrkadWeb"));
            builder.UserID = GetRequiredConfigValue("DbUsername");
            builder.Password = GetRequiredConfigValue("DbPassword");
            var connectionString = builder.ToString();
            var mysql = MySQLConfiguration.Standard.ConnectionString(connectionString);
            var configuration = OrkadWebConfigurationBuilder.Build(mysql);
            services.AddSingleton(configuration);
            var sessionFactory = configuration.BuildSessionFactory();
            services.AddSingleton(sessionFactory);
            services.AddOrkadWebMigrator("mariadb", connectionString);
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddSingleton<JwtConfig>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            services.ConfigureOptions<JwtConfig>();
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
                    Id = int.Parse(user.FindFirst("user_id").Value),
                    Name = user.FindFirst("user_name").Value,
                    Email = user.FindFirst("user_email").Value,
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpaStaticFiles();
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
