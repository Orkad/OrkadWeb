using FluentMigrator;
using OrkadWeb.Domain.Consts;
using System;

namespace OrkadWeb.Infrastructure.Persistence.Migrations
{
    [Migration(6, "user role")]
    public class UserRoleMigration : Migration
    {
        public override void Up()
        {
            Create.Table("user_role").WithColumn("id").AsAnsiString().PrimaryKey();
            Insert.IntoTable("user_role")
                .Row(new { id = "Admin" })
                .Row(new { id = "User" });
            Alter.Table("user").AddColumn("role").AsAnsiString().WithDefaultValue(UserRoles.User).NotNullable().ForeignKey("user_role", "id");
            Update.Table("user").Set(new { role = UserRoles.Admin }).Where(new { username = "admin" });
        }

        public override void Down()
        {
            Delete.Table("user_role");
            Delete.Column("role").FromTable("user");
        }
    }
}
