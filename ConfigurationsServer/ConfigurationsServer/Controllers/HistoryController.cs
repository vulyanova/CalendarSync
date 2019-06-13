using Databases;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Synchronizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<List<UILog>> GetUserLogs(string user)
        {
            var collection = GetLogCollection();

            var userCollectionCursor = await collection.FindAsync(item => item.User == user);
            var userCollection = await userCollectionCursor.ToListAsync();

            return GetUILogs(userCollection);
        }

        [EnableCors("Policy")]
        [HttpGet]
        public async Task<List<UILog>> GetLogs()
        {
            var collection = GetLogCollection();
            var userCollection = await collection.AsQueryable().ToListAsync();

            return GetUILogs(userCollection);
        }

        private IMongoCollection<Log> GetLogCollection()
        {
            var database = new MongoDatabase();
            return database.GetDatabase().GetCollection<Log>(Collection);
        }

        private List<UILog> GetUILogs(List<Log> logs)
        {
            var sorted = logs.OrderByDescending(item => item.Time);
            var uiCollection = new List<UILog>();

            foreach (var item in sorted)
                uiCollection.Add(new UILog(item));

            return uiCollection;
        }
    }
}