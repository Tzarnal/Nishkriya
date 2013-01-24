namespace Nishkriya.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScrapeStatistics : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ScraperSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        Finish = c.DateTime(nullable: false),
                        PostsAdded = c.Int(nullable: false),
                        ThreadsAdded = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ScraperSessions");
        }
    }
}
