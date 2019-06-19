using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Synchronizer.Models;

namespace SyncService.DbAdapters.MongoDbAdapter
{
    public class MongoDbAdapter : IDbInteraction
    {
        private const string Url = "https://localhost:5001/api/connection/";
        private readonly string _collection;

        public MongoDbAdapter(string collection)
        {
            _collection = collection;
        }

        public async Task Add(MainSyncItem syncAppointment)
        {
            var client = new HttpClient();
            await client.PostAsJsonAsync(Url + _collection, syncAppointment);
        }

        public async Task<List<MainSyncItem>> GetCalendarItems()
        {
            var result = new List<MainSyncItem>();
            var client = new HttpClient();
            var response = await client.GetAsync(Url + _collection);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<List<MainSyncItem>>();

            return result;
        }

        public async Task Remove(MainSyncItem syncAppointment)
        {
            var client = new HttpClient();
            await client.DeleteAsync(Url + _collection + "/"+syncAppointment.GoogleId);
        }

        public async Task Synchronize(List<MainSyncItem> syncAppointments)
        {
            var items = await GetCalendarItems();

            foreach (var syncAppointment in syncAppointments)
                if (items.All(connection => connection.GoogleId != syncAppointment.GoogleId))
                    await Add(syncAppointment);

            foreach (var item in items)
                if (syncAppointments.All(connection => connection.GoogleId != item.GoogleId))
                    await Remove(item);

        }
    }
}
