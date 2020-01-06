using System;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace RockSaltRunning.TimeTrial.Web.Models
{
    public class AthleteViewModel
    {
        public string Name { get; set; }

        public string ParkrunNumber { get; set; }

        public int EventCount { get; set; }
        public string BestPosition { get; set; }
        public string BestTime { get; set; }

        public ICollection<AthleteEventViewModel> Events { get; set; }
    }

    public class AthleteEventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Time { get; set; }
    }
}