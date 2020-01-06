using System;

namespace RockSaltRunning.TimeTrial.Web.Services
{
    public class TimeService : ITimeService
    {
        public int ConvertTimeToSeconds(string timeToConvert)
        {
            // Get the time and clean it up
            timeToConvert = timeToConvert.Replace(" AM", "").Replace(" PM", "");
            timeToConvert = timeToConvert.Substring(timeToConvert.IndexOf(' ') + 1);

            // Convert time to a span
            var time = timeToConvert.Split(':');
            var ts = new TimeSpan(0, int.Parse(time[1]), int.Parse(time[2]));

            return (int)ts.TotalSeconds;
        }
    }

    public interface ITimeService
    {
        int ConvertTimeToSeconds(string timeToConvert);
    }
}