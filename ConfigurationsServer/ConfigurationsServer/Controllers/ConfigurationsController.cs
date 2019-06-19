using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConfigurationsServer.Models;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class ConfigurationsController : Controller
    {

        [EnableCors("Policy")]
        [HttpPost]
        public async Task<MongoConfigurations> PostConfigurations([FromBody] MongoConfigurations configurations)
        {
            var database = new MongoDatabase();
            await database.AddConfigurationsAsync(configurations);
            

            return configurations;
        }

        [EnableCors("Policy")]
        [HttpGet ("{user}")]
        public async Task<MongoConfigurations> GetConfigurations(string user)
        {
            var database = new MongoDatabase();
            var configurations = await database.GetConfigurationsAsync(user);

            return configurations;
        }

        [EnableCors("Policy")]
        [HttpGet("timers")]
        public Timers[] GetTimers(string user)
        {
            var timers = new[]
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