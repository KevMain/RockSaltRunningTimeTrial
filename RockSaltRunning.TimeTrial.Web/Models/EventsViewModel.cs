using System.Collections.Generic;

namespace RockSaltRunning.TimeTrial.Web.Models
{
    public class EventsViewModel
    {
        public ICollection<EventViewModel> Events { get; set; }
    }

    public class EventViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Distance { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public int Participants { get; set; }
    }
}