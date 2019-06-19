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

        public UiLog(Log log)
        {
            User = log.User;
            Time = log.Time.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            PreviousState = new UiState(log.PreviousState);
            PresentState = new UiState(log.PresentState);

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
            }
        }
    }
}
