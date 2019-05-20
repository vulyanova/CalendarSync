using Databases;
using MongoDB.Driver;
using Synchronizer;
using System.Collections.Generic;

namespace SyncService.DbAdapters.MongoDbAdapter
{
    public class MongoDbAdapter : IDbInteraction
    {
        private IMongoCollection<MongoItem> _collection;

        public MongoDbAdapter(string collection)
        {
            var database = new MongoDatabase().GetDatabase();

            _collection = database.GetCollection<MongoItem>(collection);
        }

        public void Add(MainSyncItem syncAppointment)
        {
            var item = new MongoItem(syncAppointment);

            _collection.InsertOne(item);
        }

        public List<MainSyncItem> GetCalendarItems()
        {
            var result = new List<MainSyncItem>();
            var items = _collection.Find(item => item.GoogleId != null).ToEnumerable();

            foreach (var item in items)
            {
                result.Add(new MainSyncItem
                {
                    GoogleId = item.GoogleId,
                    OutlookId = item.OutlookId
                });
            }
            return result;
        }

        public void Remove(MainSyncItem syncAppointment)
        {
            _collection.DeleteOne(item => item.GoogleId == syncAppointment.GoogleId);
        }

        public void Save()
        { }
    }
}
