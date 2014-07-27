namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SlotActiveCreatedOnUpdatedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slot", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Slot", "CreatedOn", c => c.DateTime(nullable: true));
            Sql("UPDATE dbo.[Slot] SET CreatedOn = '2014-01-01'");
            AlterColumn("dbo.Slot", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Slot", "UpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slot", "UpdatedOn");
            DropColumn("dbo.Slot", "CreatedOn");
            DropColumn("dbo.Slot", "Active");
        }
    }
}
