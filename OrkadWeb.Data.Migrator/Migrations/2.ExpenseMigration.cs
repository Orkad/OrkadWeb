using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrkadWeb.Domain.Migrator
{

    [Migration(2)]
    public class ExpenseMigration : Migration
    {
        public override void Up()
        {
            Create.Table("expense")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("owner").AsInt32().ForeignKey("user", "id").NotNullable()
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
