namespace ConfigurationsServer.Models
{
    public class UiState
    {
        public string Subject = string.Empty;
        public string Description = string.Empty;
        public string Location = string.Empty;
        public string Attendees = string.Empty;
        public string Date = string.Empty;

        public UiState(State state, string dateTemplate)
        {
            if (state == null) return;

            Subject = state.Subject;
            Description = state.Description;
            Location = state.Location;
            Attendees = Attendees != null?string.Join(", ", state.Attendees.ToArray()):"";
            Date = state.Date.Start.ToLocalTime().ToString(dateTemplate) + " - " + state.Date.End.ToLocalTime().ToString(dateTemplate);
        }
    }
}