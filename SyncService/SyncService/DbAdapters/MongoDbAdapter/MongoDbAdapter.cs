using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using Synchronizer.Models;

namespace SyncService.DbAdapters.MongoDbAdapter
{
    public class MongoDbAdapter : IDbInteraction
    { 
        private readonly string _url;
        private readonly string _collection;

        public MongoDbAdapter(string collection)
        {
            _collection = collection;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            _url = config.AppSettings.Settings["Url"].Value+ "connection/";
        }

        public async Task Add(MainSyncItem syncAppointment)
        {
            var client = new HttpClient();
            await client.PostAsJsonAsync(_url + _collection, syncAppointment);
        }

        public async Task<List<MainSyncItem>> GetCalendarItems()
        {
            var result = new List<MainSyncItem>();
            var client = new HttpClient();
            var response = await client.GetAsync(_url + _collection);

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadAsAsync<List<MainSyncItem>>();

            return result;
        }

        public async Task Remove(MainSyncItem syncAppointment)
        {
            var client = new HttpClient();
            await client.DeleteAsync(_url + _collection + "/"+syncAppointment.GoogleId);
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
