namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PublisherActiveCreatedOnUpdatedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publisher", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Publisher", "CreatedOn", c => c.DateTime(nullable: true));
            Sql("UPDATE dbo.[Publisher] SET CreatedOn = '2014-01-01'");
            AlterColumn("dbo.Publisher", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Publisher", "UpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publisher", "UpdatedOn");
            DropColumn("dbo.Publisher", "CreatedOn");
            DropColumn("dbo.Publisher", "Active");
        }
    }
}
