namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdvertiserActiveCreatedOnUpdatedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advertiser", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Advertiser", "CreatedOn", c => c.DateTime(nullable: true));
            Sql("UPDATE dbo.[Advertiser] SET CreatedOn = '2014-01-01'");
            AlterColumn("dbo.Advertiser", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Advertiser", "UpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advertiser", "UpdatedOn");
            DropColumn("dbo.Advertiser", "CreatedOn");
            DropColumn("dbo.Advertiser", "Active");
        }
    }
}
