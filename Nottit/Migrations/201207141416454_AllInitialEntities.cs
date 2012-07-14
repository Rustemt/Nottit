namespace Nottit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllInitialEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LinkId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Links", t => t.LinkId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: false)
                .Index(t => t.LinkId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        Title = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AuthorId, cascadeDelete: false)
                .Index(t => t.AuthorId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Links", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "LinkId" });
            DropForeignKey("dbo.Links", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.Users");
            DropForeignKey("dbo.Comments", "LinkId", "dbo.Links");
            DropTable("dbo.Links");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
        }
    }
}
