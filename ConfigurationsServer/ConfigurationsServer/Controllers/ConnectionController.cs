using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigurationsServer.Models;
using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Synchronizer.Models;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class ConnectionController : Controller
    {
        [EnableCors("Policy")]
        [HttpGet ("{user}")]
        public async Task<List<MainSyncItem>> GetCalendarItems(string user)
        {
            var database = new MongoDatabase().GetDatabase();
            var collection = database.GetCollection<MongoItem>(user);

            var result = new List<MainSyncItem>();
            var items = await collection.FindAsync(FilterDefinition<MongoItem>.Empty);

            foreach (var item in items.ToEnumerable())
            {
                result.Add(new MainSyncItem
                {
                    GoogleId = item.GoogleId,
                    OutlookId = item.OutlookId,
                    TeamUpId = item.TeamUpId
                });
            }

            return result;
        }

        [EnableCors("Policy")]
        [HttpPost("{user}")]
        public async Task AddItem(string user, [FromBody] MainSyncItem item)
        {
            var database = new MongoDatabase().GetDatabase();
            var collection = database.GetCollection<MongoItem>(user);
            await collection.InsertOneAsync(new MongoItem(item));
        }

        [EnableCors("Policy")]
        [HttpDelete("{user}/{googleId}")]
        public async Task DeleteItem(string user, string googleId)
        {
            var database = new MongoDatabase().GetDatabase();
            var collection = database.GetCollection<MongoItem>(user);
            await collection.DeleteOneAsync(item=> googleId==item.GoogleId);
        }
    }
}