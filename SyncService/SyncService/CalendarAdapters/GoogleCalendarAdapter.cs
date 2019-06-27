using Calendars;
using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calendars.GoogleCalendar;
using Synchronizer.Models;

namespace SyncService.CalendarAdapters
{
    public class GoogleCalendarAdapter: ICalendar
    {
        private readonly GoogleCalendar _calendar;
        private readonly string _calendarId;
        private readonly bool _showSummary;

        public GoogleCalendarAdapter(AuthorizeConfigurations authorizeConfigurations, string calendarId, bool isPrivate)
        {
            _calendar = new GoogleCalendar(authorizeConfigurations);
            _calendarId = calendarId;
            _calendar.GetService();
            _showSummary = !isPrivate;
        }

        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
            await _calendar.DeleteAppointment(_calendarId, appointment.Id);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            var attendees = new List<EventAttendee>();
            foreach (var attendee in appointment.Attendees)
                attendees.Add(new EventAttendee { Email = attendee });

            var googleEvent = await _calendar.GetAppointment(_calendarId, appointment.Id);

            googleEvent.Summary = _showSummary ? appointment.Subject : googleEvent.Summary;
            googleEvent.Description = _showSummary ? appointment.Description : googleEvent.Description;
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
                    attendees.AddRange(item.Attendees.Select(participant => participant.Email.Trim()));

                attendees.Sort();

                if (item.Updated == null) continue;
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
            var date = new AppointmentDate();

            if (item.Start.DateTime != null)
            {
                if (item.End.DateTime != null)
                    date = new AppointmentDate((DateTime) item.Start.DateTime, (DateTime) item.End.DateTime);
            }
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
