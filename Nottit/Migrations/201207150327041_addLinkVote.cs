namespace Nottit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLinkVote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LinkVotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        VoterId = c.Int(nullable: false),
                        LinkId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.VoterId, cascadeDelete: false)
                .ForeignKey("dbo.Links", t => t.LinkId, cascadeDelete: false)
                .Index(t => t.VoterId)
                .Index(t => t.LinkId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.LinkVotes", new[] { "LinkId" });
            DropIndex("dbo.LinkVotes", new[] { "VoterId" });
            DropForeignKey("dbo.LinkVotes", "LinkId", "dbo.Links");
            DropForeignKey("dbo.LinkVotes", "VoterId", "dbo.Users");
            DropTable("dbo.LinkVotes");
        }
    }
}
