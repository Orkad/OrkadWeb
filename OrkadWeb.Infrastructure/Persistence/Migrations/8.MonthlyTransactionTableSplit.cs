using FluentMigrator;

namespace OrkadWeb.Infrastructure.Persistence.Migrations
{
    [Migration(8, "monthly transaction table split")]
    public class MonthlyTransactionTableSplit : Migration
    {
        public override void Up()
        {
            Create.Table("charge")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("owner").AsInt32().ForeignKey("user", "id").NotNullable();

            Create.Table("income")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("owner").AsInt32().ForeignKey("user", "id").NotNullable();

            Execute.Sql("insert into charge (amount, name, owner) select -amount, name, owner from monthly_transaction", "monthly_transaction to charge table");
            Execute.Sql("insert into income (amount, name, owner) select amount, name, owner from monthly_transaction", "monthly_transaction to income table");

            Delete.Table("monthly_transaction");
        }

        public override void Down()
        {
            Create.Table("monthly_transaction")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("owner").AsInt32().ForeignKey("user", "id").NotNullable();

            Execute.Sql("insert into monthly_transaction (amount, name, owner) select -amount, name, owner from charge", "charge to monthly_transaction table");
            Execute.Sql("insert into monthly_transaction (amount, name, owner) select amount, name, owner from income", "income to monthly_transaction table");

            Delete.Table("charge");
            Delete.Table("income");
        }
    }
}
