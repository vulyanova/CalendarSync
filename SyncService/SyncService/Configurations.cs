using System.Net.Http;
using System.Threading.Tasks;

namespace SyncService
{
    public class Configurations
    {
        private static Configurations _instance;
        public string CalendarId { get; }
        public int TeamUpCalendarId { get; }
        public int Timer { get;  }
        public bool ShowSummary { get;  }

        private const string Url = "https://localhost:5001/api/configurations/";

        private static async Task<Configs> GetAuthorizationConfigurationsAsync(string path)
        {
            Configs configs = null;
            var client = new HttpClient();
            var response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                configs = await response.Content.ReadAsAsync<Configs>();
            }
            return configs;
        }

        private Configurations(Configs configs)
        {
            CalendarId = configs.Calendar;
            TeamUpCalendarId = configs.TeamUpCalendar;
            Timer = configs.Timer;
            ShowSummary = configs.ShowSummary;
        }

        public static async Task GetConfigurations(string user)
        {
            var configs = await GetAuthorizationConfigurationsAsync(Url + user);
            _instance = new Configurations(configs);        
        }

        public static Configurations GetInstance()
        {
            return _instance;
        }     
    }
}
