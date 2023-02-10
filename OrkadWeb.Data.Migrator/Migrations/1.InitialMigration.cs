using FluentMigrator;
using OrkadWeb.Common;

namespace OrkadWeb.Domain.Migrator
{
    [Migration(1)]
    public class InitialMigration : Migration
    {
        public override void Up()
        {
            Create.Table("user")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("username").AsString(255).NotNullable()
                .WithColumn("password").AsString(255).NotNullable()
                .WithColumn("email").AsString(255).NotNullable();

            Insert.IntoTable("user").Row(new { username = "admin", password = Hash.Create("admin"), email = "admin@admin.com" });
        }

        public override void Down()
        {
            Delete.Table("user");
        }
    }
}
