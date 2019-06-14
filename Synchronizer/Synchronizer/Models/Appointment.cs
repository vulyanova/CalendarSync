using System;
using System.Collections.Generic;
using System.Linq;

namespace Synchronizer.Models
{
    public class Appointment
    {
        private string _subject;
        private string _location;
        private string _description;

        public enum Status { Unchecked, New, Deleted, Changed, Checked };

        public string Id { get; set; }

        public string Subject
        {
            get => _subject;
            set => _subject = value == null ? string.Empty : value.Trim();
        }

        public string Description
        {
            get => _description;
            set => _description = value == null ? string.Empty : value.Trim();
        }

        public string Location
        {
            get => _location;
            set => _location = value == null ? string.Empty : value.Trim();
        }

        public string Version { get; set; }       
        public AppointmentDate Date { get; set; }
        public List<string> Attendees { get; set; }
        public DateTime Updated { get; set; }
        public Status AppointmentStatus { get; set; } = Status.Unchecked;
        public string CreatorId { get; set; }
        public Appointment PreviousState { get; set; }

        public bool Equals(Appointment appointment, bool isPrivate)
        {
            bool areEqualPrivateFields;

            if (isPrivate)
                areEqualPrivateFields = true;
            else
                areEqualPrivateFields = Subject == appointment.Subject && Description == appointment.Description;

            var isEqualAppointment = areEqualPrivateFields &&
                Location == appointment.Location &&
                Date.Equals(appointment.Date) &&
                EqualAttendees(Attendees, appointment.Attendees);

            return isEqualAppointment;
        }

        public static bool EqualAttendees(List<string> first, List<string> second)
        {
            if (first.Count != second.Count)
                return false;

            return !first.Where((item, i) => item != second[i]).Any();
        }

        public void Update(Appointment appointment)
        {
            PreviousState = new Appointment
            {
                Subject = Subject,
                Description = Description,
                Location = Location,
                Attendees = Attendees,
                Date = new AppointmentDate(Date.Start, Date.End)
            };

            Subject = appointment.Subject;
            Description = appointment.Description;
            Location = appointment.Location;
            Date.Start = appointment.Date.Start;
            Date.End = appointment.Date.End;
            Attendees = appointment.Attendees;
        }

    }
}
