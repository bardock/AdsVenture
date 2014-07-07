namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlotEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SlotEvent",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        SlotID = c.Guid(nullable: false),
                        ContentID = c.Guid(nullable: false),
                        EventType = c.String(maxLength: 10, unicode: false),
                        PositionX = c.Int(),
                        PositionY = c.Int(),
                        TagName = c.String(maxLength: 10, unicode: false),
                        ElemId = c.String(maxLength: 50, unicode: false),
                        ElemClass = c.String(maxLength: 50, unicode: false),
                        Name = c.String(maxLength: 50, unicode: false),
                        Type = c.String(maxLength: 10, unicode: false),
                        Value = c.String(maxLength: 50, unicode: false),
                        Href = c.String(maxLength: 255, unicode: false),
                        Onclick = c.String(maxLength: 255, unicode: false),
                        Action = c.String(maxLength: 255, unicode: false),
                        Method = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Content", t => t.ContentID)
                .ForeignKey("dbo.Slot", t => t.SlotID)
                .Index(t => t.SlotID)
                .Index(t => t.ContentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SlotEvent", "SlotID", "dbo.Slot");
            DropForeignKey("dbo.SlotEvent", "ContentID", "dbo.Content");
            DropIndex("dbo.SlotEvent", new[] { "ContentID" });
            DropIndex("dbo.SlotEvent", new[] { "SlotID" });
            DropTable("dbo.SlotEvent");
        }
    }
}
