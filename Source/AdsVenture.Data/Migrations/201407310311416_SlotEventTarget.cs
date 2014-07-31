namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlotEventTarget : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SlotEventTarget",
                c => new
                    {
                        SlotEventID = c.Guid(nullable: false),
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
                .PrimaryKey(t => t.SlotEventID)
                .ForeignKey("dbo.SlotEvent", t => t.SlotEventID)
                .Index(t => t.SlotEventID);
            
            DropColumn("dbo.SlotEvent", "TagName");
            DropColumn("dbo.SlotEvent", "ElemId");
            DropColumn("dbo.SlotEvent", "ElemClass");
            DropColumn("dbo.SlotEvent", "Name");
            DropColumn("dbo.SlotEvent", "Type");
            DropColumn("dbo.SlotEvent", "Value");
            DropColumn("dbo.SlotEvent", "Href");
            DropColumn("dbo.SlotEvent", "Onclick");
            DropColumn("dbo.SlotEvent", "Action");
            DropColumn("dbo.SlotEvent", "Method");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SlotEvent", "Method", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.SlotEvent", "Action", c => c.String(maxLength: 255, unicode: false));
            AddColumn("dbo.SlotEvent", "Onclick", c => c.String(maxLength: 255, unicode: false));
            AddColumn("dbo.SlotEvent", "Href", c => c.String(maxLength: 255, unicode: false));
            AddColumn("dbo.SlotEvent", "Value", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.SlotEvent", "Type", c => c.String(maxLength: 10, unicode: false));
            AddColumn("dbo.SlotEvent", "Name", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.SlotEvent", "ElemClass", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.SlotEvent", "ElemId", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.SlotEvent", "TagName", c => c.String(maxLength: 10, unicode: false));
            DropForeignKey("dbo.SlotEventTarget", "SlotEventID", "dbo.SlotEvent");
            DropIndex("dbo.SlotEventTarget", new[] { "SlotEventID" });
            DropTable("dbo.SlotEventTarget");
        }
    }
}
