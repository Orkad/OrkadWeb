using FluentMigrator;

namespace OrkadWeb.Infrastructure.Persistence.Migrations;

[Migration(9, "Fix negatives incomes/charges")]
public class FixNegativeIncomesCharges : Migration
{
    public override void Up()
    {
        Execute.Sql("delete from charge where amount <= 0");
        Execute.Sql("delete from income where amount <= 0");
    }

    public override void Down()
    {
        // can't rollback this
    }
}