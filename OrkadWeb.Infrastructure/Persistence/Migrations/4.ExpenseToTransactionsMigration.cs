using FluentMigrator;
using System;

namespace OrkadWeb.Infrastructure.Persistence.Migrations
{
    [Migration(4, "renaming table")]
    public class ExpenseToTransactionMigration : Migration
    {
        public override void Up()
        {
            Rename.Table("expense").To("transaction");
        }

        public override void Down()
        {
            Rename.Table("transaction").To("expense");
        }
    }
}
