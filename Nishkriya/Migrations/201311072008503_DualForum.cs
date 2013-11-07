namespace Nishkriya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DualForum : DbMigration
    {
        public override void Up()
        {
            RenameColumn("ForumAccounts", "Active", "YafActive");
            AddColumn("ForumAccounts", "VbName", c => c.String(nullable: true, maxLength: 100, unicode: true));
            AddColumn("ForumAccounts", "VbActive", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("Threads", "Type", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("Posts", "PostId", c => c.Int(nullable: true));
        }

        public override void Down()
        {
            DropColumn("Posts", "PostId");
            DropColumn("Threads", "Type");
            DropColumn("ForumAccounts", "VbName");
            DropColumn("ForumAccounts", "VbActive");
            RenameColumn("ForumAccounts", "YafActive", "Active");
        }
    }
}
