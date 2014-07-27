namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlotEventImpression : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ContentImpression", "CampaignID", "dbo.Campaign");
            DropForeignKey("dbo.ContentImpression", "ContentID", "dbo.Content");
            DropForeignKey("dbo.ContentImpression", "SlotID", "dbo.Slot");
            DropIndex("dbo.ContentImpression", new[] { "ContentID" });
            DropIndex("dbo.ContentImpression", new[] { "CampaignID" });
            DropIndex("dbo.ContentImpression", new[] { "SlotID" });
            AddColumn("dbo.SlotEvent", "Discriminator", c => c.Int(nullable: false));
            AddColumn("dbo.SlotEvent", "CampaignID", c => c.Guid());
            CreateIndex("dbo.SlotEvent", "CampaignID");
            AddForeignKey("dbo.SlotEvent", "CampaignID", "dbo.Campaign", "ID");
            DropTable("dbo.ContentImpression");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ContentImpression",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ContentID = c.Guid(nullable: false),
                        CampaignID = c.Guid(nullable: false),
                        SlotID = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.SlotEvent", "CampaignID", "dbo.Campaign");
            DropIndex("dbo.SlotEvent", new[] { "CampaignID" });
            DropColumn("dbo.SlotEvent", "CampaignID");
            DropColumn("dbo.SlotEvent", "Discriminator");
            CreateIndex("dbo.ContentImpression", "SlotID");
            CreateIndex("dbo.ContentImpression", "CampaignID");
            CreateIndex("dbo.ContentImpression", "ContentID");
            AddForeignKey("dbo.ContentImpression", "SlotID", "dbo.Slot", "ID");
            AddForeignKey("dbo.ContentImpression", "ContentID", "dbo.Content", "ID");
            AddForeignKey("dbo.ContentImpression", "CampaignID", "dbo.Campaign", "ID");
        }
    }
}
