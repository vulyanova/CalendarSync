using Databases;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigurationsServer.Models;
using Synchronizer.Models;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class HistoryController : Controller
    {
        private const string Collection = "History";

        [HttpPost("{user}/{type}")]
        public async Task PushLog(string user, int type, [FromBody] Appointment appointment )
        {
            var log = new Log(user, (CalendarType)type, appointment);
            var collection = GetLogCollection();

            await collection.InsertOneAsync(log);
        }

        [HttpGet("{user}")]
        public async Task<List<UiLog>> GetUserLogs(string user)
        {
            var collection = GetLogCollection();

            var userCollectionCursor = await collection.FindAsync(item => item.User == user);
            var userCollection = await userCollectionCursor.ToListAsync();

            return userCollection.Select(item => new UiLog(item)).ToList();
        }


        [HttpGet ("{size}/{page}")]
        public async Task<List<UiLog>> GetLogs(int size, int page)
        {
            var collection = GetLogCollection();

            var userCollection = await collection
                .Find(FilterDefinition<Log>.Empty)
                .Skip((page-1) * size)
                .Limit(size)
                .Sort("{Time: -1}")
                .ToListAsync();

            return userCollection.Select(item => new UiLog(item)).ToList(); 
        }

        private static IMongoCollection<Log> GetLogCollection()
        {
            var database = new MongoDatabase();
            return database.GetDatabase().GetCollection<Log>(Collection);
        }
    }
}