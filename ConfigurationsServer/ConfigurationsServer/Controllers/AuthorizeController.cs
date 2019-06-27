using Databases;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Calendars.GoogleCalendar;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class AuthorizeController : Controller
    {
        [HttpGet]
        public string[] GetAuthorizedUsers()
        {
            var database = new MongoDatabase();
            return database.GetAuthorizedUsers().ToArray();
        }

        [HttpPost]
        public async Task<AuthorizeConfigurations> PostAuthorizeConfigurations([FromBody] Databases.AuthorizeConfigurations authorizeConfigs)
        {
            var googleCalendar = new GoogleCalendar(authorizeConfigs.ToGoogle());
            var credential = googleCalendar.Credential;

            authorizeConfigs.AccessToken = credential.Token.AccessToken;
            authorizeConfigs.RefreshToken = credential.Token.RefreshToken;

            var database = new MongoDatabase();
            await database.AddAuthorizationParametersAsync(authorizeConfigs);

            return authorizeConfigs;
        }

        [HttpGet ("{user}")]
        public async Task<Calendars.AuthorizeConfigurations> GetAuthorizeConfigurations(string user)
        {
            var database = new MongoDatabase();
            var authorizeConfigurations = await database.GetAuthorizationParametersAsync(user);

            return authorizeConfigurations.ToGoogle();
        }

        [HttpDelete("{user}")]
        public async Task DeleteAuthorizeConfigurations(string user)
        {
            var database = new MongoDatabase();
            await database.DeleteAuthorizationParametersAsync(user);
        }

    }
}
 