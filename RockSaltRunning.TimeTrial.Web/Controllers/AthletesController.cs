using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Media3D;
using RockSaltRunning.TimeTrial.Web.Helpers;
using RockSaltRunning.TimeTrial.Web.Models;

namespace RockSaltRunning.TimeTrial.Web.Controllers
{
    public class AthletesController : Controller
    {
        private readonly TimeTrialContext _db = new TimeTrialContext();

        public ActionResult Index(int id)
        {
            ViewBag.Title = "Athlete";

            var athlete = _db.Athletes.Single(x => x.Id == id);

            var bestPosition = _db.EventAthletes.Where(x => x.AthleteId == id).Include(x => x.Event).OrderBy(x => x.Position).FirstOrDefault();
            var bestTime = _db.EventAthletes.Where(x => x.AthleteId == id).Include(x => x.Event).OrderBy(x => x.Time).FirstOrDefault();

            var athleteModel = new AthleteViewModel
            {
                Name = athlete.FirstName + " " + athlete.Surname,
                ParkrunNumber = athlete.ParkrunNumber
            };

            if (bestPosition != null)
            {
                athleteModel.BestPosition = Numbers.AddOrdinal(bestPosition.Position) + " (" + bestPosition.Event.Date.ToString("dd/MM/yyyy") + ")";
            }

            if (bestTime != null)
            {
                var t = TimeSpan.FromSeconds(bestTime.Time);
                athleteModel.BestTime = $"00:{t.Minutes:D2}:{t.Seconds:D2}" + " (" + bestTime.Event.Date.ToString("dd/MM/yyyy") + ")";
            }

            athleteModel.EventCount = _db.EventAthletes.Count(x => x.AthleteId == id);

            athleteModel.Events = new List<AthleteEventViewModel>();

            var athleteEvents = _db.EventAthletes.Where(x => x.AthleteId == id).Include(x => x.Event).OrderByDescending(x => x.Event.Date);
            foreach (var athleteEvent in athleteEvents)
            {
                var t = TimeSpan.FromSeconds(athleteEvent.Time);

                athleteModel.Events.Add(new AthleteEventViewModel
                {
                    Id = athleteEvent.EventId,
                    Name = athleteEvent.Event.Name + " (" + athleteEvent.Event.Date.ToString("dd/MM/yyyy") + ")",
                    Position = Numbers.AddOrdinal(athleteEvent.Position),
                    Time = $"00:{t.Minutes:D2}:{t.Seconds:D2}"
                });
            }

            return View("Index", athleteModel);
        }

        public ActionResult Leaderboard()
        {
            ViewBag.Title = "Leaderboard";

            var leaderboardViewModel = new LeaderboardViewModel();
            leaderboardViewModel.Results = new List<LeaderboardResultsViewModel>();

            
            var athleteEvents = from t in _db.EventAthletes
                group t by t.AthleteId
                into g
                select new
                {
                    Id= g.Key,
                    Time = (from t2 in g select t2.Time).Min()
                };

            int pos = 1;
            foreach (var athleteEvent in athleteEvents.OrderBy(x => x.Time).Take(50))
            {
                var athlete = _db.Athletes.Single(x => x.Id == athleteEvent.Id);

                var bestTimeEvent = _db.EventAthletes
                    .Where(x => x.AthleteId == athlete.Id && x.Time == athleteEvent.Time).OrderBy(x => x.Event.Date).Include(x => x.Event)
                    .First();

                var t = TimeSpan.FromSeconds(athleteEvent.Time);

                leaderboardViewModel.Results.Add(new LeaderboardResultsViewModel
                {
                    Id = athlete.Id,
                    Name = athlete.FirstName + " " + athlete.Surname,
                    Date = bestTimeEvent.Event.Date.ToString("dd/MM/yyyy"),
                    Time = $"00:{t.Minutes:D2}:{t.Seconds:D2}",
                    Position = Numbers.AddOrdinal(pos)
                });

                pos = pos + 1;
            }

            return View("Leaderboard", leaderboardViewModel);
        }

        public ActionResult List()
        {
            ViewBag.Title = "Athlete List";

            var athletesListViewModel = new AthletesListViewModel {Athletes = new List<AthleteListViewModel>()};

            var athletes = _db.Athletes.OrderBy(x => x.FirstName).ThenBy(x => x.Surname);

            foreach (var athlete in athletes)
            {
                var bestTime = _db.EventAthletes.Where(x => x.AthleteId == athlete.Id).OrderBy(x => x.Time).First().Time;
                var t = TimeSpan.FromSeconds(bestTime);

                athletesListViewModel.Athletes.Add(new AthleteListViewModel
                {
                    Id = athlete.Id,
                    Name = athlete.FirstName + " " + athlete.Surname,
                    ParkrunNumber = athlete.ParkrunNumber,
                    Runs = _db.EventAthletes.Count(x => x.AthleteId == athlete.Id),
                    BestTime = $"00:{t.Minutes:D2}:{t.Seconds:D2}"
                });
            }

            return View("List", athletesListViewModel);
        }
    }
}