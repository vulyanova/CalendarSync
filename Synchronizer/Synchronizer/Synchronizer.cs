using System.Collections.Generic;
using System.Linq;
using Synchronizer.Models;

namespace Synchronizer
{
    public class Synchronizer
    {
        public List<Calendar> Calendars { get; set; }
        private readonly List<MainSyncItem> _existedAppointments;
        private readonly bool _isPrivate;

        public Synchronizer(List<Calendar> calendars, List<MainSyncItem> database, bool isPrivate)
        {
            Calendars = calendars;
            _existedAppointments = database;
            _isPrivate = isPrivate;
        }

        private void ModifyAppointment(List<Appointment> appointments)
        {
            var recentAppointment = appointments.OrderBy(item => item.Updated).Last();

            foreach (var item in appointments)
                if (item.Equals(recentAppointment, _isPrivate))
                    item.AppointmentStatus = Appointment.Status.Checked;
                else
                {
                    item.Update(recentAppointment);
                    item.AppointmentStatus = Appointment.Status.Changed;
                }
        }

        public void SyncExistedAppointments()
        {
            var deletedAppointment = new List<MainSyncItem>();

            foreach (var item in _existedAppointments)
            {
                var appointments = GetEqualAppointments(item);

                if (appointments.Contains(null))
                {
                    foreach (var appointment in appointments)
                        if (appointment != null)
                            appointment.AppointmentStatus = Appointment.Status.Deleted;

                    deletedAppointment.Add(item);
                }
                else
                    ModifyAppointment(appointments);
            }

            foreach (var item in deletedAppointment)
            {
                _existedAppointments.Remove(item);
            }
        }

        private List<Appointment> GetEqualAppointments(MainSyncItem calendarItem)
        {
            var equalAppointments = new List<Appointment>();

            foreach (var calendar in Calendars)
            {
                switch (calendar.Type)
                {
                    case CalendarType.Google:
                        equalAppointments.Add(calendar.Appointments.FirstOrDefault(gItem => gItem.Id == calendarItem.GoogleId));
                        break;
                    case CalendarType.Outlook:
                        equalAppointments.Add(calendar.Appointments.FirstOrDefault(gItem => gItem.Id == calendarItem.OutlookId));
                        break;
                    case CalendarType.TeamUp:
                        equalAppointments.Add(calendar.Appointments.FirstOrDefault(gItem => gItem.Id == calendarItem.TeamUpId));
                        break;
                }
            }

            return equalAppointments;
        }

        public void AddNewAppointments()
        {
            foreach (var calendar in Calendars)
            {
                foreach (var appointment in calendar.Appointments)
                {
                    if (appointment.AppointmentStatus == Appointment.Status.Unchecked)
                    {
                        appointment.AppointmentStatus = Appointment.Status.Checked;
                       
                        AddAppointment(appointment, calendar.Type);
                    }
                }
            }
        }

        private void AddAppointment(Appointment appointment, CalendarType type)
        {
            foreach (var calendar in Calendars)
                if (calendar.Type != type)
                    calendar.Appointments.Add(CreateAppointment(appointment));
        }

        private static Appointment CreateAppointment(Appointment appointment)
        {
            return new Appointment
            {
                AppointmentStatus = Appointment.Status.New,
                CreatorId = appointment.Id,
                Attendees = appointment.Attendees,
                Date = appointment.Date,
                Description = appointment.Description,
                Location = appointment.Location,
                Subject = appointment.Subject
            };
        }

    }
}
