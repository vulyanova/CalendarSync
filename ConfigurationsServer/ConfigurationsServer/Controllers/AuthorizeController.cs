using Calendars;
using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConfigurationsServer.Controllers
{
    public class Calendar
    {
        public string Name;
        public string Id;
    }

    [Route("api/[controller]/")]
    public class AuthorizeController : Controller
    {
        [EnableCors("Policy")]
        [HttpGet]
        public string[] GetAuthorizedUsers()
        {
            var database = new MongoDatabase();
            return database.GetAuthorizedUsers().ToArray();
        }

        [EnableCors("Policy")]
        [HttpPost]
        public async Task<Databases.AuthorizeConfigurations> PostAuthorizeConfigurations([FromBody] Databases.AuthorizeConfigurations authorizeConfigs)
        {
            var googleCalendar = new GoogleCalendar(authorizeConfigs.ToGoogle());
            var credential = googleCalendar.Credential;

            authorizeConfigs.AccessToken = credential.Token.AccessToken;
            authorizeConfigs.RefreshToken = credential.Token.RefreshToken;

            var database = new MongoDatabase();
            await database.AddAuthorizationParametersAsync(authorizeConfigs);

            return authorizeConfigs;
        }

        [EnableCors("Policy")]
        [HttpGet ("{user}")]
        public async Task<Calendars.AuthorizeConfigurations> GetAuthorizeConfigurations(string user)
        {
            var database = new MongoDatabase();
            var authorizeConfigurations = await database.GetAuthorizationParametersAsync(user);

            return authorizeConfigurations.ToGoogle();
        }

        [EnableCors("Policy")]
        [HttpDelete("{user}")]
        public async Task DeleteAuthorizeConfigurations(string user)
        {
            var database = new MongoDatabase();
            await database.DeleteAuthorizationParametersAsync(user);
        }

    }
}
 