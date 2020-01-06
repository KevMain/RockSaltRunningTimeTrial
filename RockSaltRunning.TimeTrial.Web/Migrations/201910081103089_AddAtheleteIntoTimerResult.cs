namespace RockSaltRunning.TimeTrial.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAtheleteIntoTimerResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimerResult", "AthleteId", c => c.Int());
            CreateIndex("dbo.TimerResult", "AthleteId");
            AddForeignKey("dbo.TimerResult", "AthleteId", "dbo.Athlete", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimerResult", "AthleteId", "dbo.Athlete");
            DropIndex("dbo.TimerResult", new[] { "AthleteId" });
            DropColumn("dbo.TimerResult", "AthleteId");
        }
    }
}
