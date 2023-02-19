using FluentMigrator.Runner;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrkadWeb.Angular.Config;
using OrkadWeb.Angular.Models;
using OrkadWeb.Application;
using OrkadWeb.Application.Users;
using OrkadWeb.Infrastructure;
using System;
using System.Security.Claims;
using OrkadWeb.Domain.Extensions;
using OrkadWeb.Angular.Hubs;
using MediatR;
using System.Reflection;

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
            // MVC
            services.AddControllersWithViews();
            // ANGULAR SPA
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/client-app";
            });

            // AUTHENTICATION
            services.AddSingleton<JwtConfig>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();
            services.ConfigureOptions<JwtConfig>();
            services.AddSingleton<IIdentityTokenGenerator, JwtTokenGenerator>();
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUser>(ResolveAuthenticatedUser);

            // APPLICATION + INFRASTRUCTURE
            services.AddApplicationServices();
            services.AddInfrastructureServices(Configuration);

            // HANGFIRE
            services.AddHangfire(h => h
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetRequiredValue("ConnectionStrings:Hangfire"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            services.AddHangfireServer();

            // SIGNALR
            services.AddSignalR();
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var dev = env.IsDevelopment();
            var prod = !dev;
            if (dev)
            {
                app.UseDeveloperExceptionPage();
            }
            if (prod)
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (prod)
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
                // hangfire
                endpoints.MapHangfireDashboard();
                // signalr
                endpoints.MapHub<NotificationHub>("/hub/notification");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (dev)
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
            // HANGFIRE
            app.UseHangfireDashboard();


            // DATABASE MIGRATION
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetService<IMigrationRunner>().MigrateUp();
            }
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
    }
}
