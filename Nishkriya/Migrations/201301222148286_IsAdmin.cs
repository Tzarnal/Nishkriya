namespace Nishkriya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "IsAdmin", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "Username", c => c.String(maxLength: 20));
            AlterColumn("dbo.Users", "Password", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Password", c => c.String());
            AlterColumn("dbo.Users", "Username", c => c.String(maxLength: 100));
            DropColumn("dbo.Users", "IsAdmin");
            DropColumn("dbo.Users", "Email");
        }
    }
}
