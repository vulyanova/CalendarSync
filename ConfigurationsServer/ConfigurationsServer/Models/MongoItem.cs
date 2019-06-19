using MongoDB.Bson;
using Synchronizer.Models;

namespace ConfigurationsServer.Models
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
