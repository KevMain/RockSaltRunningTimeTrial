using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockSaltRunning.TimeTrial.Web.Models
{
    public class ResultsViewModel
    {
        public EventViewModel Event { get; set; }
        public ICollection<ResultViewModel> Results { get; set; }
    }

    public class ResultViewModel
    {
        public int Id { get; set; }

        public string Position { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public string Split { get; set; }
        public string Note { get; set; }

        public int Runs { get; set; }
    }
}