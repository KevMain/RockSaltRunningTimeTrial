using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RockSaltRunning.TimeTrial.Web.Helpers;
using RockSaltRunning.TimeTrial.Web.Models;

namespace RockSaltRunning.TimeTrial.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly TimeTrialContext _db = new TimeTrialContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Events";

            var eventModel = new EventsViewModel { Events = new List<EventViewModel>() };

            foreach (var @event in _db.Events.OrderByDescending(x => x.Date).ToList())
            {
                eventModel.Events.Add(new EventViewModel
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Date = @event.Date.ToString("dd/MM/yyyy"),
                    Distance = @event.Distance,
                    Location = @event.Location,
                    Participants = _db.EventAthletes.Count(x => x.EventId == @event.Id)
                });
            }

            return View("Index", eventModel);
        }

        public ActionResult Results(int id)
        {
            ViewBag.Title = "Results";

            var @event = _db.Events.Single(x => x.Id == id);

            var resultsModel = new ResultsViewModel
            {
                Results = new List<ResultViewModel>(),
                Event = new EventViewModel
                {
                    Id = @event.Id,
                    Name = @event.Name,
                    Date = @event.Date.ToString("dd/MM/yyyy"),
                    Distance = @event.Distance,
                    Location = @event.Location,
                    Participants = _db.EventAthletes.Count(x => x.EventId == @event.Id)
                }
            };

            var eventResults = _db.EventAthletes.Where(x => x.EventId == id).OrderBy(x => x.Position).Include(x => x.Athlete).Include(x => x.Event).ToList();
            int firstTimeInSecs = eventResults[0].Time;
            foreach (var eventResult in eventResults)
            {
                var t = TimeSpan.FromSeconds(eventResult.Time);
                var s = TimeSpan.FromSeconds(eventResult.Time - firstTimeInSecs);

                var runsByAthlete = _db.EventAthletes.Where(x => x.AthleteId == eventResult.Athlete.Id && x.Event.Date < eventResult.Event.Date);

                string note;
                if (!runsByAthlete.Any())
                {
                    note = "First Timer!";
                }
                else
                {
                    var fastestRun = runsByAthlete.Where(x => x.Time < eventResult.Time).OrderBy(x => x.Time);

                    if (fastestRun.Any())
                    {
                        var t2 = TimeSpan.FromSeconds(fastestRun.First().Time);
                        note = "PB stays at " + $"00:{t2.Minutes:D2}:{t2.Seconds:D2}";
                    }
                    else
                    {
                        note = "New PB!";
                    }
                }

                resultsModel.Results.Add(new ResultViewModel
                {
                    Id = eventResult.Athlete.Id,
                    Name = eventResult.Athlete.FirstName + " " + eventResult.Athlete.Surname,
                    Position = Numbers.AddOrdinal(@eventResult.Position),
                    Split = $"{s.Hours:D2}:{s.Minutes:D2}:{s.Seconds:D2}",
                    Time = $"00:{t.Minutes:D2}:{t.Seconds:D2}",
                    Note = note,
                    Runs = runsByAthlete.Count() + 1
                });
            }

            return View("Results", resultsModel);
        }
    }
}