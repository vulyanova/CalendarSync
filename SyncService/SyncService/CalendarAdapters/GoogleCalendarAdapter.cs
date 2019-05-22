using Calendars;
using Google.Apis.Calendar.v3.Data;
using Synchronizer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncService.CalendarAdapters
{
    public class GoogleCalendarAdapter: ICalendar
    {
        private static GoogleCalendarAdapter _instance;
        private GoogleCalendar _calendar;
        private string _calendarId = "primary";

        private GoogleCalendarAdapter(AuthorizeConfigurations authorizeConfigurations, string calendarId)
        {
            _calendar = new GoogleCalendar(authorizeConfigurations);
            _calendarId = calendarId;
            _calendar.GetService();
        }

        public static void Authorize(AuthorizeConfigurations authorizeConfigurations, string calendarId)
        {
            _instance = new GoogleCalendarAdapter(authorizeConfigurations, calendarId);
        }

        public static GoogleCalendarAdapter GetInstance()
        {
            return _instance;
        }

        public async Task UpdateAsync(List<Appointment> appointments)
        {
            foreach (var item in appointments)
            {
                if (item.AppointmentStatus == Appointment.Status.New)
                    item.Id = await AddAppointmentAsync(item);

                if (item.AppointmentStatus == Appointment.Status.Deleted)
                    await DeleteAppointmentAsync(item.Id);

                if (item.AppointmentStatus == Appointment.Status.Changed)
                    await UpdateAppointmentAsync(item);
            }
        }

        public async Task DeleteAppointmentAsync(string id)
        {
            await _calendar.DeleteAppointment(_calendarId, id);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            var attendees = new List<EventAttendee>();
            foreach (var attendee in appointment.Attendees)
                attendees.Add(new EventAttendee { Email = attendee });

            var googleEvent = await _calendar.GetAppointment(_calendarId, appointment.Id);
            var showSummary = true;

            googleEvent.Summary = showSummary ? appointment.Subject : googleEvent.Summary;
            googleEvent.Description = showSummary ? appointment.Description : googleEvent.Description;
            googleEvent.Location = appointment.Location;
            googleEvent.Start.DateTime = appointment.Date.Start;
            googleEvent.End.DateTime = appointment.Date.End;
            googleEvent.Attendees = attendees;

            await _calendar.UpdateAppointment(_calendarId, googleEvent);
        }

        public async Task<List<Appointment>> GetNearestAppointmentsAsync()
        {
            var events = await _calendar.GetNearestAppointmentsAsync(_calendarId);

            var list = new List<Appointment>();

            foreach (var item in events.Items)
            {
                var attendees = new List<string>();

                if (item.Attendees != null)
                    foreach (var participant in item.Attendees)
                        attendees.Add(participant.Email.Trim());

                attendees.Sort();

                var newEvent = new Appointment()
                {
                    Id = item.Id,
                    Subject = item.Summary,
                    Description = item.Description,
                    Location = item.Location,
                    Date = GetDateTime(item),
                    Updated = (DateTime)item.Updated,
                    Attendees = attendees
                };

                list.Add(newEvent);
            }

            return list;
        }

        private static AppointmentDate GetDateTime(Event item)
        {
            AppointmentDate date;

            if (item.Start.DateTime != null)
                date = new AppointmentDate((DateTime)item.Start.DateTime, (DateTime)item.End.DateTime);
            else
                date = new AppointmentDate(item.Start.Date, item.End.Date);

            return date;
        }

        public async Task<string> AddAppointmentAsync(Appointment appointment)
        {
            var attendees = new List<EventAttendee>();

            foreach (var attendee in appointment.Attendees)
                attendees.Add(new EventAttendee { Email = attendee });

            var newEvent = new Event
            {
                Summary = appointment.Subject,
                Description = appointment.Description,
                Location = appointment.Location,
                Start = new EventDateTime { DateTime = appointment.Date.Start },
                End = new EventDateTime { DateTime = appointment.Date.End },
                Attendees = attendees
            };

            return await _calendar.AddAppointment(_calendarId, newEvent);
        }

    }
}
