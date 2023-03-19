using FluentMigrator;

namespace OrkadWeb.Infrastructure.Persistence.Migrations
{
    [Migration(7, "unique user")]
    public class UniqueUserMigration : Migration
    {
        public override void Up()
        {
            Alter.Table("user").AlterColumn("username").AsString(255).NotNullable().Unique();
        }

        public override void Down()
        {
            Alter.Table("user").AlterColumn("username").AsString(255).NotNullable();
        }
    }
}
