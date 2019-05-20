using Calendars;
using Synchronizer;
using SyncService.DbAdapters.MongoDbAdapter;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SyncService
{
    public static class SyncController
    {
        static readonly string url = "https://localhost:5001/api/authorize/";

        static async Task<AuthorizeConfigurations> GetAuthorizationConfigurationsAsync(string path)
        {
            AuthorizeConfigurations configs = null;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                configs = await response.Content.ReadAsAsync<AuthorizeConfigurations>();
            }
            return configs;
        }

        public static async Task Sync(string user)
        {
            var authorizationParams = await GetAuthorizationConfigurationsAsync(url+user);

            var configuratons = Configurations.GetInstance();

            GoogleCalendarAdapter.Authorize(authorizationParams, configuratons.CalendarId);
            var googleCalendar = GoogleCalendarAdapter.GetInstance();
            var outlookCalendar = OutlookCalendarAdapter.GetInstance();

            var db = new MongoDbAdapter(user);

            var syncAppointments = db.GetCalendarItems();
            var googleAppointments = await googleCalendar.GetNearestAppointmentsAsync();
            var outlookAppointments = outlookCalendar.GetNearestAppointments();

            var calendars = new List<Calendar>
                {
                    new Calendar
                    {
                        Appointments = googleAppointments,
                        Type = CalendarType.Google
                    },

                    new Calendar
                    {
                        Appointments = outlookAppointments,
                        Type = CalendarType.Outlook
                    }
                };

            var synchronizer = new Synchronizer.Synchronizer(calendars, syncAppointments, !configuratons.ShowSummary);

            synchronizer.SyncExistedAppointments();

            synchronizer.AddNewAppointments();

            await googleCalendar.UpdateGoogleAsync(synchronizer.Calendars.Find(item => item.Type == CalendarType.Google).Appointments);

            outlookCalendar.UpdateOutlook(synchronizer.Calendars.Find(item => item.Type == CalendarType.Outlook).Appointments);

            DbSync.Sync(db, synchronizer.Calendars);

        }
    }
}
