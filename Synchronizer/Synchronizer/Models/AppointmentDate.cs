using System;

namespace Synchronizer.Models
{
    public class AppointmentDate
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public AppointmentDate()
        {}

        public AppointmentDate(string start, string end)
        {
            Start = DateTime.Parse(start, System.Globalization.CultureInfo.InvariantCulture);
            End = DateTime.Parse(end, System.Globalization.CultureInfo.InvariantCulture);
        }

        public AppointmentDate(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public bool Equals(AppointmentDate date)
        {
            return Start == date.Start && End == date.End;
        }
    }
}
