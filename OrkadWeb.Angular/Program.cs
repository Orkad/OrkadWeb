using FluentMigrator.Runner;
using Hangfire;
using Hangfire.MySql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using OrkadWeb.Angular.Config;
using OrkadWeb.Angular.Hubs;
using OrkadWeb.Angular.Models;
using OrkadWeb.Application;
using OrkadWeb.Application.Users;
using OrkadWeb.Infrastructure;
using OrkadWeb.Infrastructure.Extensions;
using OrkadWeb.Infrastructure.Injection;
using System;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Transactions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var dev = builder.Environment.IsDevelopment();
var prod = !dev;
// MVC
services.AddControllersWithViews();
// ANGULAR SPA
services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

// AUTHENTICATION
services.AddSingleton<JwtConfig>();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer();
services.ConfigureOptions<JwtConfig>();
services.AddSingleton<IIdentityTokenGenerator, JwtTokenGenerator>();
services.AddSession();
services.AddHttpContextAccessor();
services.AddScoped<IAppUser, HttpAppUser>();
services.AddSingleton<IServiceProviderProxy, HttpContextServiceProviderProxy>();

// APPLICATION + INFRASTRUCTURE
services.AddApplicationServices();
services.AddInfrastructureServices(configuration);

var mysql = new MySqlConnectionStringBuilder(configuration.GetRequiredValue("ConnectionStrings:OrkadWeb"));
mysql.AllowUserVariables = true;
// HANGFIRE
services.AddHangfire(h => h
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(new MySqlStorage(
        mysql.ToString(),
        new MySqlStorageOptions
        {
            TransactionIsolationLevel = IsolationLevel.ReadCommitted,
            QueuePollInterval = TimeSpan.FromSeconds(15),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromMinutes(5),
            PrepareSchemaIfNecessary = true,
            DashboardJobListLimit = 50000,
            TransactionTimeout = TimeSpan.FromMinutes(1),
            TablesPrefix = "Hangfire"
        })));
services.AddHangfireServer();

// SIGNALR
services.AddSignalR();

var app = builder.Build();
ServiceLocator.Initialize(app.Services.GetService<IServiceProviderProxy>());
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
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<IMigrationRunner>().MigrateUp();
}

app.MapFallbackToFile("index.html");

app.Run();