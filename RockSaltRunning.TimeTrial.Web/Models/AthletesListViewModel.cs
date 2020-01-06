using System;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace RockSaltRunning.TimeTrial.Web.Models
{
    public class AthletesListViewModel
    {
        public ICollection<AthleteListViewModel> Athletes { get; set; }
    }

    public class AthleteListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ParkrunNumber { get; set; }
        public int Runs { get; set; }
        public string BestTime { get; set; }
    }
}