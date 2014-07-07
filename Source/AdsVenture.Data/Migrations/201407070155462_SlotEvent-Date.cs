namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlotEventDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SlotEvent", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SlotEvent", "Date");
        }
    }
}
