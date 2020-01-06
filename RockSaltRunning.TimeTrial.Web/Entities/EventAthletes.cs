using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RockSaltRunning.TimeTrial.Web.Entities
{
    public class EventAthletes
    {
        [ForeignKey("Event")]
        public int EventId { get; set; }

        [ForeignKey("Athlete")]
        public int AthleteId { get; set; }

        public int Position { get; set; }

        public int Time { get; set; }

        public Event Event { get; set; }

        public Athlete Athlete { get; set; }
    }
}