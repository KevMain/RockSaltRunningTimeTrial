using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RockSaltRunning.TimeTrial.Web.Entities;
using RockSaltRunning.TimeTrial.Web.Models;
using RockSaltRunning.TimeTrial.Web.Services;

namespace RockSaltRunning.TimeTrial.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly TimeTrialContext _db = new TimeTrialContext();
        private readonly ITimeService _timeService;

        public ImportController()
        {
            _timeService = new TimeService();
        }

        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Map(int id)
        {
            var importedEvent = _db.Imports.SingleOrDefault(x => x.Id == id);

            var importViewModel = new ImportViewModel
            {
                Id = id,
                Date = importedEvent.EventDate.ToString("dd/MM/yyyy"),
                IsScannerUploaded = importedEvent.IsScannerUploaded,
                IsMappingComplete = !_db.TimerResults.Any(x => x.ImportId == id && !x.AthleteId.HasValue)
            };

            var timerResults  =_db.TimerResults.Where(x => x.ImportId == id).OrderBy(x => x.Position).ToList();
            importViewModel.TimerResults = new List<TimerResultViewModel>();
            foreach (var timerResult in timerResults)
            {
                var t = TimeSpan.FromSeconds(timerResult.Time);

                importViewModel.TimerResults.Add(new TimerResultViewModel
                {
                    AthleteId = timerResult.AthleteId ?? 0,
                    Id = timerResult.Id,
                    Position = timerResult.Position,
                    Time = $"00:{t.Minutes:D2}:{t.Seconds:D2}"
                });
            }

            var athletes = _db.Athletes.OrderBy(x => x.FirstName).ThenBy(x => x.Surname);
            importViewModel.Athletes = new List<AthleteModel>();
            foreach (var athlete in athletes)
            {
                importViewModel.Athletes.Add(new AthleteModel
                {
                    Id = athlete.Id,
                    Name = athlete.FirstName + " " + athlete.Surname,
                    ParkrunNumber = athlete.ParkrunNumber
                });
            }

            return View("Map", importViewModel);
        }


        public ActionResult Upload(FormCollection formCollection)
        {
            if (DateTime.TryParse(Request.Form["eventDate"], out var eventDate))
            {
                var import = new Import
                {
                    EventDate = eventDate,
                    ImportDate = DateTime.Now
                };

                _db.Imports.Add(import);

                var file = Request.Files["UploadedFile"];

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    CreateImportFromFile(file, import);
                }

                _db.SaveChanges();

                return RedirectToAction("Map", new { id = import.Id });
            }

            return View("Index"); 
        }


        public ActionResult Scanner(int id, FormCollection formCollection)
        {
            var file = Request.Files["UploadedFile"];

            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                using (var reader = new StreamReader(file.InputStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var values = line.Split(',');
                            var parkrunNumber = values[0];

                            var matchingAthlete = _db.Athletes.SingleOrDefault(x => x.ParkrunNumber == parkrunNumber);
                            if (matchingAthlete != null)
                            {
                                var position = int.Parse(values[1].Remove(0,1));

                                var timerResult = _db.TimerResults.SingleOrDefault(
                                    x => x.ImportId == id && x.Position == position);

                                if (timerResult != null)
                                {
                                    timerResult.AthleteId = matchingAthlete.Id;

                                    _db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }

            var importedResult = _db.Imports.Single(x => x.Id == id);
            importedResult.IsScannerUploaded = true;
            _db.SaveChanges();

            return RedirectToAction("Map", new { id = id });
        }

        public ActionResult Results(int id, FormCollection formCollection)
        {
            var timerResults = _db.TimerResults.Where(x => x.ImportId == id).ToList();

            foreach (var timerResult in timerResults)
            {
                var selectBox = formCollection[timerResult.Id.ToString()];
                if (selectBox != null)
                {
                    int athleteId = 0;
                    if (int.TryParse(selectBox, out athleteId))
                    {
                        timerResult.AthleteId = athleteId;
                        _db.SaveChanges();
                    }

                }
            }

            return RedirectToAction("Map", new { id = id });
        }

        public ActionResult Publish(int id, FormCollection formCollection)
        {
            var importedEvent = _db.Imports.Single(x => x.Id == id);

            var eventResult = _db.Events.Add(new Event
            {
                Date = importedEvent.EventDate,
                Distance = "1.9 Miles",
                Location = "Carey Park",
                Name = "Time Trial"
            });

            _db.SaveChanges();

            var timerResults = _db.TimerResults.Where(x => x.ImportId == id);

            foreach (var timerResult in timerResults)
            {
                _db.EventAthletes.Add(new EventAthletes
                {
                    AthleteId = timerResult.AthleteId.HasValue ? timerResult.AthleteId.Value : 0,
                    EventId = eventResult.Id,
                    Position = timerResult.Position,
                    Time = timerResult.Time
                });
            }

            _db.SaveChanges();

            _db.TimerResults.RemoveRange(timerResults);
            _db.Imports.Remove(importedEvent);

            _db.SaveChanges();

            return RedirectToAction("Results", "Events", new { id = eventResult.Id });
        }

        private void CreateImportFromFile(HttpPostedFileBase file, Import import)
        {
            using (var reader = new StreamReader(file.InputStream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null)
                    {
                        CreateTimerResultFromRow(line, import);
                    }
                }
            }
        }

        private void CreateTimerResultFromRow(string line, Import import)
        {
            var values = line.Split(',');

            if (int.TryParse(values[0], out var position) && position > 0)
            {
                var timerResult = new TimerResult
                {
                    Position = position,
                    Time = _timeService.ConvertTimeToSeconds(values[2]),
                    Import = import
                };

                _db.TimerResults.Add(timerResult);
            }
        }
    }
}