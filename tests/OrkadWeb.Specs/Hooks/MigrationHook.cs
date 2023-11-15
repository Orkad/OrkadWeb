using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using OrkadWeb.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Specs.Hooks
{
    [Binding]
    public class MigrationHook
    {
        [BeforeTestRun]
        public static void BeforeTestRun(IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();
        }

        [AfterTestRun]
        public static void AfterTestRun(IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateDown(0);
        }
    }

    [Migration(long.MaxValue, "test initialize")]
    public class TestInitializeMigration : Migration
    {
        public override void Up()
        {
            Delete.FromTable("user").AllRows();
        }

        public override void Down() { }
    }

    public static class IMigrationHookExtensions
    {
        public static IServiceCollection AddTestMigrations(this IServiceCollection services, string connectionString)
            => services.AddFluentMigratorCore()
                .ConfigureRunner(b => b
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(OrkadWebInfrastructure.Assembly, Assembly.GetExecutingAssembly()));
    }
}
