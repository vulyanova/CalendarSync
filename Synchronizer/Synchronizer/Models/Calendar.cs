using System.Collections.Generic;

namespace Synchronizer.Models
{
    public enum CalendarType { Google, Outlook, TeamUp };

    public class Calendar
    {
        public List<Appointment> Appointments { get; set; }
        public CalendarType Type { get; set; }
    }
}
