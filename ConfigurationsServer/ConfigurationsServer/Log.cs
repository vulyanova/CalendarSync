using MongoDB.Bson;
using System;
using Synchronizer.Models;

namespace ConfigurationsServer
{
    public class Log
    {
        public enum AppointmentAction { Added, Deleted, Updated };

        public ObjectId Id;
        public CalendarType Calendar;
        public string AppointmentId;
        public DateTime Time;
        public string User;
        public AppointmentAction Action;
        public State PreviousState;
        public State PresentState;

        public Log(string user, CalendarType type, Appointment appointment)
        {
            Calendar = type;
            AppointmentId = appointment.Id;
            User = user;
            Time = DateTime.Now;

            switch (appointment.AppointmentStatus)
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
}
