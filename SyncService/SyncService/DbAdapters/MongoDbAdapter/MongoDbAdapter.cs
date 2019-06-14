using Databases;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Synchronizer.Models;

namespace SyncService.DbAdapters.MongoDbAdapter
{
    public class MongoDbAdapter : IDbInteraction
    {
        private readonly IMongoCollection<MongoItem> _collection;

        public MongoDbAdapter(string collection)
        {
            var database = new MongoDatabase().GetDatabase();

            _collection = database.GetCollection<MongoItem>(collection);
        }

        public async Task Add(MainSyncItem syncAppointment)
        {
            var item = new MongoItem(syncAppointment);

            await _collection.InsertOneAsync(item);
        }

        public async Task<List<MainSyncItem>> GetCalendarItems()
        {
            var result = new List<MainSyncItem>();
            var items = await _collection.FindAsync(item => item.GoogleId != null);

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

        public async Task Remove(MainSyncItem syncAppointment)
        {
            await _collection.DeleteOneAsync(item => item.GoogleId == syncAppointment.GoogleId);
        }

        public async Task Save()
        {
            await Task.CompletedTask;
        }

        public async Task Synchronize(List<MainSyncItem> syncAppointments)
        {
            var items = await GetCalendarItems();

            foreach (var item in items)
                if (syncAppointments.All(connection => connection.GoogleId != item.GoogleId))
                    await Remove(item);

            foreach (var syncAppointment in syncAppointments)
                if (items.All(connection => connection.GoogleId != syncAppointment.GoogleId))
                    await Add(syncAppointment);

        }
    }
}
