using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Calendars.TeamUpCalendar;
using Synchronizer;

namespace SyncService.CalendarAdapters
{
    public class TeamUpCalendarAdapter : ICalendar
    {
        private TeamUpCalendar _calendar;

        public TeamUpCalendarAdapter(string calendarKey, int calendarId)
        {
            _calendar = new TeamUpCalendar(calendarKey, calendarId);
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

            await _calendar.AddAppointment(teamUpAppointment);

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

            await _calendar.DeleteAppointment(teamUpAppointment);
        }

        public async Task<List<Appointment>> GetNearestAppointmentsAsync()
        {
            var result = new List<Appointment>();

            var appointments = await _calendar.GetNearestAppointments();
            foreach(var teamUpAppointment in appointments)
            {
                var attendees = teamUpAppointment.Who.Split(',').ToList();
                attendees?.Sort();

                var appointment = new Appointment
                {
                    Id = teamUpAppointment.Id,
                    Location = teamUpAppointment.Location,
                    Date = new AppointmentDate(teamUpAppointment.Start, teamUpAppointment.End),
                    Updated = teamUpAppointment.Update,
                    Version = teamUpAppointment.Version,
                    Subject = teamUpAppointment.Title,
                    Attendees = attendees!= null?attendees: new List<string>(),
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

            await _calendar.UpdateAppointment(teamUpAppointment);

            appointment.Version = teamUpAppointment.Version;
        }

    }
}
