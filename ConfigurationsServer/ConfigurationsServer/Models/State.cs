using System.Collections.Generic;
using Synchronizer.Models;

namespace ConfigurationsServer.Models
{
    public class State
    {
        public string Subject;
        public string Description;
        public string Location;
        public List<string> Attendees;
        public AppointmentDate Date;

        public State(Appointment appointment)
        {
            Subject = appointment.Subject;
            Description = appointment.Description;
            Location = appointment.Location;
            Date = new AppointmentDate(appointment.Date.Start, appointment.Date.End);
            Attendees = appointment.Attendees;
        }
    }
}