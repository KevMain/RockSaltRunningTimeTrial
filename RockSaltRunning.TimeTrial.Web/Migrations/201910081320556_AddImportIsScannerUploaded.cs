namespace RockSaltRunning.TimeTrial.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImportIsScannerUploaded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Import", "IsScannerUploaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Import", "IsScannerUploaded");
        }
    }
}
