using System;

namespace HeiaMeg.Utils
{
    public static class TimeUtils
    {
        public static string ToStringNorwegian(this DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Friday:
                    return "Fredag";
                case DayOfWeek.Monday:
                    return "Mandag";
                case DayOfWeek.Saturday:
                    return "Lørdag";
                case DayOfWeek.Sunday:
                    return "Søndag";
                case DayOfWeek.Thursday:
                    return "Torsdag";
                case DayOfWeek.Tuesday:
                    return "Tirsdag";
                case DayOfWeek.Wednesday:
                    return "Onsdag";
                default:
                    throw new ArgumentOutOfRangeException(nameof(day), day, null);
            }
        }

        private static readonly Random Random = new Random();

        /// <summary>
        /// Gets a random time within the timeframe (minute precision)
        /// </summary>
        public static DateTime RandomTimeWithinTimeSpan(DateTime start, TimeSpan from, TimeSpan to)
        {
            var diff = to - from;
            var seconds = diff.TotalSeconds > 0 ? Random.Next(0, (int)diff.TotalSeconds) : 0;
            return start.Date.Add(from).AddSeconds(seconds);
        }
    }
}