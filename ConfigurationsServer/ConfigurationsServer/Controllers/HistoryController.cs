using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchronizer.Models;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class HistoryController : Controller
    {
        private const string Collection = "History";
        [EnableCors("Policy")]
        [HttpPost("{user}/{type}")]
        public async Task PushLog(string user, int type, [FromBody] Appointment appointment )
        {
            var log = new Log(user, (CalendarType)type, appointment);
            var collection = GetLogCollection();

            await collection.InsertOneAsync(log);
        }

        [EnableCors("Policy")]
        [HttpGet("{user}")]
        public async Task<List<UiLog>> GetUserLogs(string user)
        {
            var collection = GetLogCollection();

            var userCollectionCursor = await collection.FindAsync(item => item.User == user);
            var userCollection = await userCollectionCursor.ToListAsync();

            return GetUiLogs(userCollection);
        }

        [EnableCors("Policy")]
        [HttpGet]
        public async Task<List<UiLog>> GetLogs()
        {
            var collection = GetLogCollection();
            var userCollection = await collection.AsQueryable().ToListAsync();

            return GetUiLogs(userCollection);
        }

        private static IMongoCollection<Log> GetLogCollection()
        {
            var database = new MongoDatabase();
            return database.GetDatabase().GetCollection<Log>(Collection);
        }

        private static List<UiLog> GetUiLogs(IEnumerable<Log> logs)
        {
            var sorted = logs.OrderByDescending(item => item.Time);

            return sorted.Select(item => new UiLog(item)).ToList();
        }
    }
}