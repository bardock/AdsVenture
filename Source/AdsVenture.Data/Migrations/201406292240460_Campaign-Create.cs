namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampaignCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Campaign",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50, unicode: false),
                        AdvertiserID = c.Guid(nullable: false),
                        EndsOn = c.DateTime(),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedOn = c.DateTime(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Advertiser", t => t.AdvertiserID)
                .Index(t => t.AdvertiserID);
            
            AddColumn("dbo.ContentImpression", "CampaignID", c => c.Guid(nullable: false));
            AddColumn("dbo.ContentImpression", "SlotID", c => c.Guid(nullable: false));
            CreateIndex("dbo.ContentImpression", "ContentID");
            CreateIndex("dbo.ContentImpression", "CampaignID");
            CreateIndex("dbo.ContentImpression", "SlotID");
            AddForeignKey("dbo.ContentImpression", "CampaignID", "dbo.Campaign", "ID");
            AddForeignKey("dbo.ContentImpression", "ContentID", "dbo.Content", "ID");
            AddForeignKey("dbo.ContentImpression", "SlotID", "dbo.Slot", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContentImpression", "SlotID", "dbo.Slot");
            DropForeignKey("dbo.ContentImpression", "ContentID", "dbo.Content");
            DropForeignKey("dbo.ContentImpression", "CampaignID", "dbo.Campaign");
            DropForeignKey("dbo.Campaign", "AdvertiserID", "dbo.Advertiser");
            DropIndex("dbo.ContentImpression", new[] { "SlotID" });
            DropIndex("dbo.ContentImpression", new[] { "CampaignID" });
            DropIndex("dbo.ContentImpression", new[] { "ContentID" });
            DropIndex("dbo.Campaign", new[] { "AdvertiserID" });
            DropColumn("dbo.ContentImpression", "SlotID");
            DropColumn("dbo.ContentImpression", "CampaignID");
            DropTable("dbo.Campaign");
        }
    }
}
