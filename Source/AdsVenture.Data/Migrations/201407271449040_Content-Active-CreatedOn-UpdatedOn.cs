namespace AdsVenture.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContentActiveCreatedOnUpdatedOn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Content", "CreatedOn", c => c.DateTime(nullable: true));
            Sql("UPDATE dbo.[Content] SET CreatedOn = '2014-01-01'");
            AlterColumn("dbo.Content", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Content", "UpdatedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Content", "UpdatedOn");
            DropColumn("dbo.Content", "CreatedOn");
            DropColumn("dbo.Content", "Active");
        }
    }
}
