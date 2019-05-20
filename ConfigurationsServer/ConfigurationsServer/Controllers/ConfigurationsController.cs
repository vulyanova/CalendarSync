using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class ConfigurationsController : Controller
    {

        [EnableCors("Policy")]
        [HttpPost]
        public MongoConfigurations PostConfigurations([FromBody] MongoConfigurations configurations)
        {
            var database = new MongoDatabase();
            database.AddConfigurations(configurations);

            return configurations;
        }

        [EnableCors("Policy")]
        [HttpGet ("{user}")]
        public MongoConfigurations GetConfigurations(string user)
        {
            var database = new MongoDatabase();
            var configurations = database.GetConfigurations(user);

            return configurations;
        }

        [EnableCors("Policy")]
        [HttpGet("timers")]
        public Timers[] GetTimers(string user)
        {
            var timers = new Timers[]
            {
                new Timers { Name = "1 minute", Ms = 60000 },
                new Timers { Name = "10 minutes", Ms = 600000 },
                new Timers { Name = "30 minutes", Ms = 1800000 },
                new Timers { Name = "1 hour", Ms = 3600000 },
            };

            return timers;
        }
      
    }
}