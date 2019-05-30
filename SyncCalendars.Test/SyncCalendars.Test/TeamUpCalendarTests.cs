using Calendars.TeamUpCalendar;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SyncCalendars.Test
{
    public class TeamUpCalendarTests
    {
        [Fact]
        public async Task NewAppointments_AddCorrectAmountAsync()
        {
            var teamUpCalendar = new TeamUpCalendar("0ad07f8905ca44f73a62048fcf3aaf7c485dec5c036d5647806daa4bb6157b94", "ksjea1t78n1525ka23", 6524793);

            var start = DateTime.Now.AddDays(1);
            var appointment = new TeamUpEvent
            {
                Location = "Yaroslavl",
                Title = "event",
                Start = start,
                End = start.AddMinutes(90),
                Description = "Appointment description",
                Who = "chuvaginavika@gmail.com,chuvaginavika@icloud.com"
            };

            await teamUpCalendar.AddAppointment(appointment);

            appointment.Location = "Moscow";
            await teamUpCalendar.UpdateAppointment(appointment); 

            var result = await teamUpCalendar.GetNearestAppointments();

            Assert.True(result.Where(item => item.Id == appointment.Id && item.Location == "Moscow").Count()>0);

           /* await teamUpCalendar.DeleteAppointment(appointment);
            result = await teamUpCalendar.GetNearestAppointments();

            Assert.True(result.Where(item => item.Id == appointment.Id).Count() == 0);*/
        }
    }
}
