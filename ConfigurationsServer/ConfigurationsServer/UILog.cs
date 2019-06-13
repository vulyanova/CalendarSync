using System;

namespace ConfigurationsServer
{
    public class UILog
    {
        public string User;
        public string Calendar;
        public string Time;
        public string Action;
        public UIState PreviousState;
        public UIState PresentState;

        public UILog(Log log)
        {
            User = log.User;
            Time = log.Time.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
            PreviousState = new UIState(log.PreviousState);
            PresentState = new UIState(log.PresentState);

            switch (log.Calendar)
            {
                case Synchronizer.CalendarType.Google:
                    Calendar = "Google";
                    break;
                case Synchronizer.CalendarType.Outlook:
                    Calendar = "Outlook";
                    break;
                case Synchronizer.CalendarType.TeamUp:
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

    public class UIState
    {
        public string Subject = "";
        public string Description = "";
        public string Location = "";
        public string Attendees = "";
        public string Date = "";

        public UIState(State state)
        {
            if (state!=null)
            {
                Subject = state.Subject?.ToString();
                Description = state.Description?.ToString();
                Location = state.Location?.ToString();
                Attendees = Attendees != null?string.Join(", ", state.Attendees.ToArray()):"";
                Date = state.Date.Start.ToString("dd/MM/yyyy hh:mm") + " - " + state.Date.End.ToString("dd/MM/yyyy hh:mm");
            }
        }
    }
}
