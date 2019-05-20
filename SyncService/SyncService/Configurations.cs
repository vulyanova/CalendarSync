using System.Net.Http;
using System.Threading.Tasks;

namespace SyncService
{
    public class Configurations
    {
        private static Configurations _instance = null;
        public string CalendarId { get; private set; }
        public int Timer { get; private set; }
        public bool ShowSummary { get; private set; }

        private static readonly string url = "https://localhost:5001/api/configurations/";

        private static async Task<Configs> GetAuthorizationConfigurationsAsync(string path)
        {
            Configs configs = null;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                configs = await response.Content.ReadAsAsync<Configs>();
            }
            return configs;
        }

        private Configurations(Configs configs)
        {
            CalendarId = configs.Calendar;
            Timer = configs.Timer;
            ShowSummary = configs.ShowSummary;
        }

        public static async Task GetConfigurations(string user)
        {
            var configs = await GetAuthorizationConfigurationsAsync(url + user);
            _instance = new Configurations(configs);        
        }

        public static Configurations GetInstance()
        {
            return _instance;
        }     
    }
}
