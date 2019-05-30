using MongoDB.Bson;
using Synchronizer;

namespace SyncService.DbAdapters.MongoDbAdapter
{
    public class MongoItem
    {
        public ObjectId Id;
        public string GoogleId;
        public string OutlookId;
        public string TeamUpId;

        public MongoItem(MainSyncItem item)
        {
            GoogleId = item.GoogleId;
            OutlookId = item.OutlookId;
            TeamUpId = item.TeamUpId;
            Id = ObjectId.GenerateNewId();
        }
    }
}
