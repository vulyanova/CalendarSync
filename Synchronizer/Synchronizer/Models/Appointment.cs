using System;
using System.Collections.Generic;

namespace Synchronizer
{
    public class Appointment
    {
        public enum Status { Unchecked, New, Deleted, Changed, Checked };

        public string Id { get; set; }
        public string Subject { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public AppointmentDate Date { get; set; }
        public List<string> Attendees { get; set; }
        public DateTime Updated { get; set; }
        public Status AppointmentStatus { get; set; } = Status.Unchecked;
        public string CreatorId { get; set; }

        public bool Equals(Appointment appointment, bool isPrivate)
        {
            bool isEqualPrivateFields;
            if (isPrivate) isEqualPrivateFields = true;
            else isEqualPrivateFields = (Subject == appointment.Subject && Description == appointment.Description);

            if (isEqualPrivateFields &&
                Location == appointment.Location &&
                Date.Equals(appointment.Date) &&
                EqualAttendees(Attendees, appointment.Attendees))
                return true;

            return false;
        }

        public static bool EqualAttendees(List<string> first, List<string> second)
        {
            if (first.Count != second.Count)
                return false;

            for (var i = 0; i < first.Count; i++)
                if (first[i] != second[i])
                    return false;

            return true;
        }

        public void Update(Appointment appointment)
        {
            Subject = appointment.Subject;
            Description = appointment.Description;
            Location = appointment.Location;
            Date.Start = appointment.Date.Start;
            Date.End = appointment.Date.End;
            Attendees = appointment.Attendees;
        }

    }
}
