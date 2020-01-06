using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RockSaltRunning.TimeTrial.Web.Entities;
using RockSaltRunning.TimeTrial.Web.Models;
using OfficeOpenXml;

namespace RockSaltRunning.TimeTrial.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly TimeTrialContext _db = new TimeTrialContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Upload(FormCollection formCollection)
        {
            if (Request != null)
            {
                var usersList = new List<ImportResult>();
                var dt = new DateTime();

                HttpPostedFileBase file = Request.Files["UploadedFile"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                    
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;

                        string[] dates = workSheet.Name.Split('_');

                        dt = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            var user = new ImportResult
                            {
                                ParkrunNumber = workSheet.Cells[rowIterator, 1].Value != null ? workSheet.Cells[rowIterator, 1].Value.ToString() : "",
                                Position = workSheet.Cells[rowIterator, 2].Value.ToString(),
                                Time = workSheet.Cells[rowIterator, 3].Value.ToString(),
                                Name = workSheet.Cells[rowIterator, 4].Value.ToString()
                            };
                            usersList.Add(user);
                        }
                    }
                }

                if(!_db.Events.Any(x => x.Date == dt))
                {
                    _db.Events.Add(new Event
                    {
                        Date = dt,
                        Distance = "1.9 Miles",
                        Location = "Carey Park",
                        Name = "Time Trial"
                    });
                }

                foreach (var user in usersList)
                {
                    string[] names = user.Name.Split(' ');

                    var firstname = names[0];
                    var surname = String.Empty;
                    for (var i = 1; i < names.Length; i++)
                    {
                        surname += names[i];
                        if (i != names.Length - 1)
                            surname += " ";
                    }

                    if (!String.IsNullOrEmpty(user.ParkrunNumber))
                    {
                        if (!_db.Athletes.Any(x => x.ParkrunNumber == user.ParkrunNumber))
                        {
                            if (!_db.Athletes.Any(x => x.FirstName == firstname && x.Surname == surname))
                            {
                                _db.Athletes.Add(new Athlete
                                {
                                    FirstName = firstname,
                                    Surname = surname,
                                    ParkrunNumber = user.ParkrunNumber
                                });
                            }
                            else
                            {
                                var currentAthlete = _db.Athletes.Single(x => x.FirstName == firstname && x.Surname == surname);
                                currentAthlete.ParkrunNumber = user.ParkrunNumber;
                            }
                        }
                    }
                    else
                    {
                        if (!_db.Athletes.Any(x => x.FirstName == firstname && x.Surname == surname))
                        {
                            _db.Athletes.Add(new Athlete
                            {
                                FirstName = firstname,
                                Surname = surname,
                                ParkrunNumber = String.Empty
                            });
                        }
                    }
                }
                _db.SaveChanges();

                var eventRun = _db.Events.Single(x => x.Date == dt);
                foreach (var result in usersList)
                {

                    Athlete athlete;
                    if (!String.IsNullOrEmpty(result.ParkrunNumber))
                    {
                        athlete = _db.Athletes.Single(x => x.ParkrunNumber == result.ParkrunNumber);
                    }
                    else
                    {
                        string[] names = result.Name.Split(' ');

                        var firstname = names[0];
                        var surname = String.Empty;
                        for (var i = 1; i < names.Length; i++)
                        {
                            surname += names[i];
                            if (i != names.Length - 1)
                                surname += " ";
                        }

                        athlete = _db.Athletes.Single(x => x.FirstName == firstname && x.Surname == surname);
                    }

                    if (!_db.EventAthletes.Any(x => x.EventId == eventRun.Id && x.AthleteId == athlete.Id))
                    {
                        try
                        {
                            result.Time = result.Time.Replace(" AM", "").Replace(" PM", "");
                            result.Time = result.Time.Substring(result.Time.IndexOf(' ') + 1);
                            var time = result.Time.Split(':');
                            var ts = new TimeSpan(0, int.Parse(time[1]), int.Parse(time[2]));

                            _db.EventAthletes.Add(new EventAthletes
                            {
                                AthleteId = athlete.Id,
                                EventId = eventRun.Id,
                                Position = int.Parse(result.Position),
                                Time = (int) ts.TotalSeconds
                            });
                        }
                        catch (Exception ex)
                        {
                            Response.Write(ex.Message + " = " + result.Time.ToString());
                        }
                    }

                }

                _db.SaveChanges();

            }
            return View("Index");
        }
    }
}