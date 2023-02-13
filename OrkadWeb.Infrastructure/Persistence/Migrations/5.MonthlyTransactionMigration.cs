using FluentMigrator;
using System;

namespace OrkadWeb.Infrastructure.Persistence.Migrations
{
    [Migration(5, "monthly transaction")]
    public class MonthlyTransactionMigration : Migration
    {
        public override void Up()
        {
            Create.Table("monthly_transaction")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("name").AsString(255).NotNullable()
                .WithColumn("owner").AsInt32().ForeignKey("user", "id").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("monthly_transaction");
        }
    }
}
