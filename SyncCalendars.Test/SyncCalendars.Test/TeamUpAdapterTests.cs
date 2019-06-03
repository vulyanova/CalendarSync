using Synchronizer;
using SyncService.CalendarAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SyncCalendars.Test
{
    public class TeamUpAdapterTests
    {
        [Fact]
        public async Task NewAppointments_AddCorrectAmountAsync()
        {
            var teamUpAdapter = new TeamUpCalendarAdapter("ksjea1t78n1525ka23", 6551483);

            var start = DateTime.Now.AddDays(1);
            var appointment = new Appointment
            {
                Location = "Yaroslavl",
                Subject = "test event",
                Date = new AppointmentDate(start, start.AddMinutes(90)),
                Description = "Appointment description",
                Attendees = new List<string> { "chuvaginavika@gmail.com", "chuvaginavika@icloud.com" }
            };

            await teamUpAdapter.AddAppointmentAsync(appointment);

            appointment.Location = "Moscow";
            await teamUpAdapter.UpdateAppointmentAsync(appointment);

            var result = await teamUpAdapter.GetNearestAppointmentsAsync();

            Assert.True(result.Where(item => item.Id == appointment.Id && item.Location == "Moscow").Count() > 0);
        }
    }
}
