namespace Nishkriya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrepareForMultiForum : DbMigration
    {
        public override void Up()
        {
            RenameTable("Posts", "YafPosts");
            RenameTable("Threads", "YafThreads");
            RenameColumn("ForumAccounts", "ForumId", "YafId");
        }
        
        public override void Down()
        {
            RenameColumn("ForumAccounts", "YafId", "ForumId");
            RenameTable("YafThreads", "Threads");
            RenameTable("YafPosts", "Posts");
        }
    }
}
