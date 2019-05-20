using System;

namespace Synchronizer
{
    public class AppointmentDate
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public AppointmentDate(string start, string end)
        {
            Start = DateTime.ParseExact(start, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            End = DateTime.ParseExact(end, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        public AppointmentDate(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Equals(AppointmentDate date)
        {
            if (Start == date.Start && End == date.End)
                return true;

            return false;
        }
    }
}
