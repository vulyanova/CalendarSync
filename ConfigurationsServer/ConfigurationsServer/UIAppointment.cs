using System;

namespace ConfigurationsServer
{
    public class UIAppointment
    {
        public string Subject;
        public string Description;
        public string Date;
        public string Attendees;
    }

    public class UICalendar
    {
        public string Name;
        public UIAppointment[] Previous;
        public UIAppointment[] Present;
    }
}
