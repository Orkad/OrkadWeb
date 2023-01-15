using FluentMigrator.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace OrkadWeb.Tests.Hooks
{
    [Binding]
    public class MigrationHook
    {
        [BeforeTestRun]
        public static void BeforeTestRun(IMigrationRunner migrationRunner)
        {
            migrationRunner.MigrateUp();
        }
    }
}
