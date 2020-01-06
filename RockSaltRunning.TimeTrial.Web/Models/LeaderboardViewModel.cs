using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockSaltRunning.TimeTrial.Web.Models
{
    public class LeaderboardViewModel
    {
        public ICollection<LeaderboardResultsViewModel> Results { get; set; }
        
    }

    public class LeaderboardResultsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
        public string Position { get; set; }
    }
}