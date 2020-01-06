using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RockSaltRunning.TimeTrial.Web.Entities;

namespace RockSaltRunning.TimeTrial.Web
{
    public class TimeTrialContext : DbContext
    {
        public TimeTrialContext() : base("DefaultConnection")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<EventAthletes> EventAthletes { get; set; }

        public DbSet<Import> Imports { get; set; }
        public DbSet<TimerResult> TimerResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<EventAthletes>().HasKey(c => new { c.EventId, c.AthleteId });
        }
    }
}