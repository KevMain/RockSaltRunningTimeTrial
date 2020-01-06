namespace RockSaltRunning.TimeTrial.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewImportTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Import",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        ImportDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Import");
        }
    }
}
