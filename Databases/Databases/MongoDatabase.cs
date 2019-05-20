using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Databases
{
    public class MongoDatabase
    {
        private const string Client = "mongodb+srv://newadmin:02072012@calendarcluster-tjsbr.gcp.mongodb.net/test?retryWrites=true";
        private const string Database = "SyncConfigurations";
        private const string AuthorizationCollection = "Authorization";
        private const string ConnectionCollection = "Connection";
        private const string ConfigurationsCollection = "Configurations";
        private IMongoDatabase _database;

        public MongoDatabase()
        {
            var client = new MongoClient(Client);
            _database = client.GetDatabase(Database);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }

        public List<string> GetAuthorizedUsers()
        {
            var collection = _database.GetCollection<AuthorizeConfigurations>(AuthorizationCollection);

            var list = collection.AsQueryable().Select(item => item.User).ToList();

            return list;
        }

        public void AddAuthorizationParameters(AuthorizeConfigurations authorizeConfigurations)
        {
            var collection = _database.GetCollection<AuthorizeConfigurations>(AuthorizationCollection);
            var record = collection.Find(item => item.User == authorizeConfigurations.User).FirstOrDefault();

            if (record == null)
                collection.InsertOne(authorizeConfigurations);
            else
                collection.ReplaceOne(item => item.User == authorizeConfigurations.User, authorizeConfigurations);
        }

        public AuthorizeConfigurations GetAuthorizationParameters(string user)
        {
            var collection = _database.GetCollection<AuthorizeConfigurations>(AuthorizationCollection);
            var record = collection.Find(item => item.User == user).FirstOrDefault();

            return record;
        }

        public MongoConfigurations GetConfigurations(string user)
        {
            var collection = _database.GetCollection<MongoConfigurations>(ConfigurationsCollection);
            var record = collection.Find(item => item.User == user).FirstOrDefault();

            return record;
        }

        public void AddConfigurations(MongoConfigurations configurations)
        {
            var collection = _database.GetCollection<MongoConfigurations>(ConfigurationsCollection);
            collection.DeleteMany(item => item.User == configurations.User);
            collection.InsertOne(configurations);

            var collections = _database.ListCollectionNames().ToEnumerable();
            if (!collections.Contains(configurations.User))
                _database.CreateCollection(configurations.User);

        }

    }
}
