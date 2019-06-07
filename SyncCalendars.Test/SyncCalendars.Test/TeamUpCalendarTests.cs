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
            var teamUpCalendar = new TeamUpCalendar("ksjea1t78n1525ka23", 6551483);
            var attendees = "chuvaginavika@gmail.com,chuvaginavika@icloud.com";

            var start = DateTime.Now.AddDays(1);
            var appointment = new TeamUpEvent
            {
                Location = "Yaroslavl",
                Title = "test event",
                Start = start,
                End = start.AddMinutes(90),
                Description = "Appointment description",
                Who = attendees
            };

            await teamUpCalendar.AddAppointment(appointment);

            appointment.Location = "Moscow";
            await teamUpCalendar.UpdateAppointment(appointment); 

            var result = await teamUpCalendar.GetNearestAppointments();

            Assert.True(result.Where(item => 
            item.Id == appointment.Id && 
            item.Location == "Moscow" &&
            item.Who == attendees).Count()>0);    
        }
    }
}
