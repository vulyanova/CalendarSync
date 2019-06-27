using System.Threading.Tasks;
using Calendars;
using Calendars.TeamUpCalendar;
using Databases;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class TeamUpCalendarsController : Controller
    {
        [HttpGet("{user}")]
        public async Task<Calendar[]> GetCalendars(string user)
        {
            var database = new MongoDatabase();
            var authorizeConfigs = await database.GetAuthorizationParametersAsync(user);

            var teamUpCalendar = new TeamUpCalendar(authorizeConfigs.CalendarKey);
            var calendars = await teamUpCalendar.GetCalendars();

            return calendars.ToArray();
        }
    }

}
    


