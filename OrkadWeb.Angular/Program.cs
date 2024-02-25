using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
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
using System.Transactions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var dev = builder.Environment.IsDevelopment();
var prod = !dev;

// MVC
services.AddControllersWithViews();

// AUTHENTICATION
if (dev)
{
    services.AddOrkadWebCors();
}
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

// SIGNALR
services.AddSignalR();
services.AddHealthChecks();

// DOCUMENTATION
services.AddOpenApiDocument();

var app = builder.Build();
ServiceLocator.Initialize(app.Services.GetService<IServiceProviderProxy>());
if (prod)
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

if (dev)
{
    app.UseCors(CorsConfiguration.DEFAULT_POLICY);
}
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.UseSession();
app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
app.MapHub<NotificationHub>("/hub/notification");
app.MapHealthChecks("/health");
app.MapFallbackToFile("index.html");

// DATABASE MIGRATION
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<IMigrationRunner>().MigrateUp();
}

app.Run();