using System.Threading.Tasks;
using Calendars;
using Calendars.GoogleCalendar;
using Databases;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class CalendarsController : Controller
    {
        [HttpGet("{user}")]
        public async Task<Calendar[]> GetCalendars(string user)
        {
            var database = new MongoDatabase();
            var authorizeConfigs = await database.GetAuthorizationParametersAsync(user);

            var googleCalendar = new GoogleCalendar(authorizeConfigs.ToGoogle());
            var calendars = await googleCalendar.GetCalendars();

            return calendars.ToArray();
        }

    }
}