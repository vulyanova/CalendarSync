using Calendars;
using Synchronizer;
using SyncService.CalendarAdapters;
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

            if (configuratons.ShowSummary)
                outlookCalendar.ShowSummary();
            else outlookCalendar.HideSummary();
            
            var db = new MongoDbAdapter(user);

            var syncAppointments = db.GetCalendarItems();
            var googleAppointments = await googleCalendar.GetNearestAppointmentsAsync();
            var outlookAppointments = await outlookCalendar.GetNearestAppointmentsAsync();

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

            await googleCalendar.UpdateAsync(synchronizer.Calendars.Find(item => item.Type == CalendarType.Google).Appointments);

            await outlookCalendar.UpdateAsync(synchronizer.Calendars.Find(item => item.Type == CalendarType.Outlook).Appointments);

            DbSync.Sync(db, synchronizer.Calendars);

        }
    }
}
