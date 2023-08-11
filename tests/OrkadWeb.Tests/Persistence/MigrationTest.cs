using System.Data;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using NFluent;
using OrkadWeb.Infrastructure;

namespace OrkadWeb.Tests.Persistence
{
    [TestClass]
    public class MigrationTest
    {
        [TestMethod]
        public void RunMigrationUpFullTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(
                    builder => builder
                        .AddSQLite()
                        .WithGlobalConnectionString(@"Data Source=:memory:;Version=3;New=True;")
                        .WithMigrationsIn(OrkadWebInfrastructure.Assembly))
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();

                DataSet dataSet = runner.Processor.Read("SELECT Description FROM VersionInfo", string.Empty);
                Check.That(dataSet).IsNotNull();
                Check.That(dataSet.Tables[0].Rows[0].ItemArray[0]).IsEqualTo("InitialMigration");
            }
        }
    }
}
