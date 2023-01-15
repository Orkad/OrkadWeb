using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace OrkadWeb.Data.Migrator
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddOrkadWebMigrator(this IServiceCollection services, string databaseType, string connectionString)
        {
            return services.AddFluentMigratorCore()
                .ConfigureRunner(builder =>
                {
                    switch (databaseType)
                    {
                        case "mysql":
                        case "mariadb":
                            builder.AddMySql5();
                            break;
                        case "sqlite":
                            builder.AddSQLite();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    builder
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }
    }
}

