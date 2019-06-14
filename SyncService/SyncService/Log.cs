using System;
using System.Collections.Generic;
using Synchronizer.Models;

namespace SyncService
{
    public class Log
    {
        public enum AppointmentAction { Added, Deleted, Updated};

        public CalendarType Calendar;
        public string AppointmentId;
        public DateTime Time;
        public string User;
        public AppointmentAction Action;
        public State PreviousState;
        public State PresentState;

        public Log(string user, Appointment.Status status, CalendarType type, Appointment appointment)
        {
            Calendar = type;
            AppointmentId = appointment.Id;
            User = user;
            Time = DateTime.Now;

            switch (status)
            {
                case Appointment.Status.New:
                    Action = AppointmentAction.Added;
                    PresentState = new State(appointment);
                    break;
                case Appointment.Status.Deleted:
                    Action = AppointmentAction.Deleted;
                    PreviousState = new State(appointment);
                    break;
                case Appointment.Status.Changed:
                    Action = AppointmentAction.Updated;
                    PreviousState = new State(appointment.PreviousState);
                    PresentState = new State(appointment);
                    break;
            } 
        }

    }

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
