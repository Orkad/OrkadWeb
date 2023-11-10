using FluentMigrator;

namespace OrkadWeb.Infrastructure.Persistence.Migrations;

[Migration(10, "Set transaction to negatives")]
public class SetTransactionToNegativesMigration : Migration
{
    public override void Up()
    {
        Execute.Sql(@"update `transaction` set amount = -amount");
    }

    public override void Down()
    {
        Execute.Sql(@"update `transaction` set amount = -amount");
    }
}