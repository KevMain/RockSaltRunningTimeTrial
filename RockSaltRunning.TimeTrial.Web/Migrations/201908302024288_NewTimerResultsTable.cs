namespace RockSaltRunning.TimeTrial.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTimerResultsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimerResult",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImportId = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Time = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Import", t => t.ImportId, cascadeDelete: true)
                .Index(t => t.ImportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimerResult", "ImportId", "dbo.Import");
            DropIndex("dbo.TimerResult", new[] { "ImportId" });
            DropTable("dbo.TimerResult");
        }
    }
}
