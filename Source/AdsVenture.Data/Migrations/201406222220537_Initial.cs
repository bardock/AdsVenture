namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertiser",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        CountryID = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        ID = c.Short(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsoCode = c.String(nullable: false, maxLength: 2, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.IsoCode, unique: true);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 255, unicode: false),
                        CountryID = c.Short(),
                        LanguageID = c.Short(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .ForeignKey("dbo.Language", t => t.LanguageID)
                .Index(t => t.CountryID)
                .Index(t => t.LanguageID);
            
            CreateTable(
                "dbo.Language",
                c => new
                    {
                        ID = c.Short(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsoCode = c.String(maxLength: 3, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.IsoCode, unique: true);
            
            CreateTable(
                "dbo.ContentImpression",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ContentID = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Content",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, unicode: false),
                        AdvertiserID = c.Guid(nullable: false),
                        Url = c.String(nullable: false, maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Advertiser", t => t.AdvertiserID)
                .Index(t => t.AdvertiserID);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        CountryID = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Slot",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, unicode: false),
                        PublisherID = c.Guid(nullable: false),
                        ContentID = c.Guid(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Content", t => t.ContentID)
                .ForeignKey("dbo.Publisher", t => t.PublisherID)
                .Index(t => t.PublisherID)
                .Index(t => t.ContentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Slot", "PublisherID", "dbo.Publisher");
            DropForeignKey("dbo.Slot", "ContentID", "dbo.Content");
            DropForeignKey("dbo.Publisher", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Content", "AdvertiserID", "dbo.Advertiser");
            DropForeignKey("dbo.Advertiser", "CountryID", "dbo.Country");
            DropForeignKey("dbo.User", "LanguageID", "dbo.Language");
            DropForeignKey("dbo.User", "CountryID", "dbo.Country");
            DropIndex("dbo.Slot", new[] { "ContentID" });
            DropIndex("dbo.Slot", new[] { "PublisherID" });
            DropIndex("dbo.Publisher", new[] { "CountryID" });
            DropIndex("dbo.Content", new[] { "AdvertiserID" });
            DropIndex("dbo.Language", new[] { "IsoCode" });
            DropIndex("dbo.User", new[] { "LanguageID" });
            DropIndex("dbo.User", new[] { "CountryID" });
            DropIndex("dbo.Country", new[] { "IsoCode" });
            DropIndex("dbo.Advertiser", new[] { "CountryID" });
            DropTable("dbo.Slot");
            DropTable("dbo.Publisher");
            DropTable("dbo.Content");
            DropTable("dbo.ContentImpression");
            DropTable("dbo.Language");
            DropTable("dbo.User");
            DropTable("dbo.Country");
            DropTable("dbo.Advertiser");
        }
    }
}
