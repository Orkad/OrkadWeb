using FluentMigrator;
using System;

namespace OrkadWeb.Data.Migrator
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
