using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Data.Migrator
{
    [Migration(1)]
    public class AddExpenseTable : Migration
    {
        public override void Up()
        {
            Create.Table("expense")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("owner").AsInt32().ForeignKey()
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("name").AsString(255)
                .WithColumn("date").AsDate().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("expense");
        }
    }
}
