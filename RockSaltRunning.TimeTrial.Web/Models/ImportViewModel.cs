using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockSaltRunning.TimeTrial.Web.Models
{
    public class ImportViewModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public bool IsScannerUploaded { get; set; }

        public bool IsMappingComplete { get; set; }

        public ICollection<TimerResultViewModel> TimerResults { get; set; }
        public ICollection<AthleteModel> Athletes { get; set; }
    }

    public class TimerResultViewModel
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Time { get; set; }
        public int AthleteId { get; set; }
    }

    public class AthleteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParkrunNumber { get; set; }
    }
}