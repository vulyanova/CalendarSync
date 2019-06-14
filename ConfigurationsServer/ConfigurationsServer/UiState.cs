namespace ConfigurationsServer
{
    public class UiState
    {
        public string Subject = "";
        public string Description = "";
        public string Location = "";
        public string Attendees = "";
        public string Date = "";

        public UiState(State state)
        {
            if (state == null) return;

            Subject = state.Subject;
            Description = state.Description;
            Location = state.Location;
            Attendees = Attendees != null?string.Join(", ", state.Attendees.ToArray()):"";
            Date = state.Date.Start.ToLocalTime().ToString("dd/MM/yyyy HH:mm") + " - " + state.Date.End.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
        }
    }
}