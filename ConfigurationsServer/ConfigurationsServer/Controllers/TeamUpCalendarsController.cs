using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class TeamUpCalendarsController : Controller
    {
        [EnableCors("Policy")]
        [HttpGet("{user}")]
        public async Task<Calendar[]> GetCalendars(string user)
        {
            var database = new MongoDatabase();
            var authorizeConfigs = await database.GetAuthorizationParametersAsync(user);

            var webRequest = WebRequest.Create("https://api.teamup.com/" + authorizeConfigs.CalendarKey + "/subcalendars");

            webRequest.Method = "GET";
            webRequest.Timeout = 12000;
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add("Teamup-Token", "0ad07f8905ca44f73a62048fcf3aaf7c485dec5c036d5647806daa4bb6157b94");

            var response = await webRequest.GetResponseAsync();

            using (System.IO.Stream s = response.GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    var jsonResponse = sr.ReadToEnd();
                    dynamic des = JsonConvert.DeserializeObject(jsonResponse);
                    var list = new List<Calendar>();
                    foreach (var appointment in des.subcalendars)
                    {
                        var calendar = new Calendar()
                        {
                            Id = appointment.id,
                            Name = appointment.name
                        };

                        list.Add(calendar);
                    }

                    return list.ToArray();
                }

            }
        }
    }

}
    


