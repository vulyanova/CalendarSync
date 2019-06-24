using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calendars.TeamUpCalendar;
using Synchronizer.Models;

namespace SyncService.CalendarAdapters
{
    public class TeamUpCalendarAdapter : ICalendar
    {
        private readonly TeamUpCalendar _calendar;
        private readonly int _calendarId;

        public TeamUpCalendarAdapter(string calendarKey, int calendarId)
        {
            _calendarId = calendarId;
            _calendar = new TeamUpCalendar(calendarKey);
        }
        public async Task<string> AddAppointmentAsync(Appointment appointment)
        {
            var teamUpAppointment = new TeamUpEvent
            {
                Id = appointment.Id,
                Location = appointment.Location,
                Start = appointment.Date.Start,
                End = appointment.Date.End,
                Title = appointment.Subject,
                Description = appointment.Description,
                Who = string.Join(",", appointment.Attendees.ToArray())
            };

            await _calendar.AddAppointment(_calendarId, teamUpAppointment);

            appointment.Id = teamUpAppointment.Id;
            appointment.Version = teamUpAppointment.Version;

            return appointment.Id;
        }

        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
            var teamUpAppointment = new TeamUpEvent
            {
                Id = appointment.Id,
                Location = appointment.Location,
                Version = appointment.Version,
                Start = appointment.Date.Start,
                End = appointment.Date.End,
                Title = appointment.Subject,
                Who = string.Join(",", appointment.Attendees.ToArray()),
                Description = appointment.Description
            };

            await _calendar.DeleteAppointment(_calendarId, teamUpAppointment);
        }

        public async Task<List<Appointment>> GetNearestAppointmentsAsync()
        {
            var result = new List<Appointment>();
            var appointments = await _calendar.GetNearestAppointments(_calendarId);

            foreach(var teamUpAppointment in appointments)
            {
                var attendees = new List<string>();
                if (teamUpAppointment.Who != string.Empty)
                    attendees = teamUpAppointment.Who.Split(',').ToList();

                var attendeesEmail = attendees.Where(item=> item.Contains("@")).Select(s => s.Trim()).OrderBy(item=> item).ToList();

                var appointment = new Appointment
                {
                    Id = teamUpAppointment.Id,
                    Location = teamUpAppointment.Location,
                    Date = new AppointmentDate(teamUpAppointment.Start, teamUpAppointment.End),
                    Updated = teamUpAppointment.Update,
                    Version = teamUpAppointment.Version,
                    Subject = teamUpAppointment.Title,
                    Attendees = attendeesEmail,
                    Description = teamUpAppointment.Description,
                };

                result.Add(appointment);
            }

            return result;
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            var teamUpAppointment = new TeamUpEvent
            {
                Id = appointment.Id,
                Location = appointment.Location,
                Version = appointment.Version,
                Start = appointment.Date.Start,
                End = appointment.Date.End,
                Title = appointment.Subject,
                Description = appointment.Description,
                Who = string.Join(",", appointment.Attendees.ToArray())
            };

            await _calendar.UpdateAppointment(_calendarId, teamUpAppointment);

            appointment.Version = teamUpAppointment.Version;
        }

    }
}
