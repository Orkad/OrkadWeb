using FluentMigrator;
using System;

namespace OrkadWeb.Data.Migrator
{
    [Migration(3, "add user informations")]
    public class UserInformationMigration : Migration
    {
        public override void Up()
        {
            Create.Column("creation").OnTable("user").AsDateTime().SetExistingRowsTo(DateTime.Now).NotNullable();
            Create.Column("confirmation").OnTable("user").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("creation").FromTable("user");
            Delete.Column("confirmation").FromTable("user");
        }
    }
}
