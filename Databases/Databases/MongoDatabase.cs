using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task DeleteAuthorizationParametersAsync(string user)
        {
            var collection = _database.GetCollection<AuthorizeConfigurations>(AuthorizationCollection);
            await collection.DeleteManyAsync(item => item.User == user);      
        }

        public async Task AddAuthorizationParametersAsync(AuthorizeConfigurations authorizeConfigurations)
        {
            var collection = _database.GetCollection<AuthorizeConfigurations>(AuthorizationCollection);
            await collection.DeleteManyAsync(item => item.User == authorizeConfigurations.User);

            await collection.InsertOneAsync(authorizeConfigurations);
        }

        public async Task<AuthorizeConfigurations> GetAuthorizationParametersAsync(string user)
        {
            var collection = _database.GetCollection<AuthorizeConfigurations>(AuthorizationCollection);
            var records = await collection.FindAsync(item => item.User == user);

            return records.FirstOrDefault();
        }

        public async Task<MongoConfigurations> GetConfigurationsAsync(string user)
        {
            var collection = _database.GetCollection<MongoConfigurations>(ConfigurationsCollection);
            var records = await collection.FindAsync(item => item.User == user);

            return records.FirstOrDefault();
        }

        public async Task AddConfigurationsAsync(MongoConfigurations configurations)
        {
            var collection = _database.GetCollection<MongoConfigurations>(ConfigurationsCollection);
            await collection.DeleteManyAsync(item => item.User == configurations.User);
            await collection.InsertOneAsync(configurations);

            var collections = await _database.ListCollectionNamesAsync();

            if (!collections.ToEnumerable().Contains(configurations.User))
                await _database.CreateCollectionAsync(configurations.User);

        }

    }
}
