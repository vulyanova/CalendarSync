using System;
using Synchronizer.Models;

namespace ConfigurationsServer.Models
{
    public class UiLog
    {
        public string User;
        public string Calendar;
        public string Time;
        public string Action;
        public UiState PreviousState;
        public UiState PresentState;
        private const string DateTemplate = "dd/MM/yyyy HH:mm";

        public UiLog(Log log)
        {
            User = log.User;
            Time = log.Time.ToLocalTime().ToString(DateTemplate);
            PreviousState = new UiState(log.PreviousState, DateTemplate);
            PresentState = new UiState(log.PresentState, DateTemplate);

            switch (log.Calendar)
            {
                case CalendarType.Google:
                    Calendar = "Google";
                    break;
                case CalendarType.Outlook:
                    Calendar = "Outlook";
                    break;
                case CalendarType.TeamUp:
                    Calendar = "TeamUp";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (log.Action)
            {
                case Log.AppointmentAction.Added:
                    Action = "Added";
                    break;
                case Log.AppointmentAction.Deleted:
                    Action = "Deleted";
                    break;
                case Log.AppointmentAction.Updated:
                    Action = "Updated";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
