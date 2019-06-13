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
        static readonly string url = "https://localhost:5001/api/";

        static async Task PushLogs(string user, int type, Appointment appointment)
        {
            var client = new HttpClient();
            await client.PostAsJsonAsync(url + "history/" + user + "/" + type, appointment);
        }

        static async Task<AuthorizeConfigurations> GetAuthorizationConfigurationsAsync(string user)
        {
            AuthorizeConfigurations configs = null;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url + "authorize/" + user);
            if (response.IsSuccessStatusCode)
                configs = await response.Content.ReadAsAsync<AuthorizeConfigurations>();

            return configs;
        }

        private static async Task UpdateCalendar(string user, ICalendar calendar, Calendar appointmentCalendar)
        {
            var appointments = appointmentCalendar.Appointments;

            foreach (var item in appointments)
            {
                if (item.AppointmentStatus == Appointment.Status.Checked)
                    continue;

                if (item.AppointmentStatus == Appointment.Status.New)
                    item.Id = await calendar.AddAppointmentAsync(item);

                if (item.AppointmentStatus == Appointment.Status.Deleted)
                    await calendar.DeleteAppointmentAsync(item);

                if (item.AppointmentStatus == Appointment.Status.Changed)
                    await calendar.UpdateAppointmentAsync(item);

                await PushLogs(user, (int)appointmentCalendar.Type, item);
            }
        }

        public static async Task Sync(string user)
        {
            var authorizationParams = await GetAuthorizationConfigurationsAsync(user);

            var configuratons = Configurations.GetInstance();

            GoogleCalendarAdapter.Authorize(authorizationParams, configuratons.CalendarId);
            var googleCalendar = GoogleCalendarAdapter.GetInstance();
            var outlookCalendar = OutlookCalendarAdapter.GetInstance();
            var teamUpCalendar = new TeamUpCalendarAdapter(authorizationParams.CalendarKey, configuratons.TeamUpCalendarId);

            if (configuratons.ShowSummary)
                outlookCalendar.ShowSummary();
            else
                outlookCalendar.HideSummary();
            
            var db = new MongoDbAdapter(user);

            var syncAppointments = await db.GetCalendarItems();
            var googleAppointments = await googleCalendar.GetNearestAppointmentsAsync();
            var outlookAppointments = await outlookCalendar.GetNearestAppointmentsAsync();
            var teamUpAppointments = await teamUpCalendar.GetNearestAppointmentsAsync();

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
                    },
                     new Calendar
                    {
                        Appointments = teamUpAppointments,
                        Type = CalendarType.TeamUp
                    },
                };

            var synchronizer = new Synchronizer.Synchronizer(calendars, syncAppointments, !configuratons.ShowSummary);

            synchronizer.SyncExistedAppointments();

            synchronizer.AddNewAppointments();

            await UpdateCalendar(user, googleCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.Google));
            await UpdateCalendar(user, outlookCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.Outlook));
            await UpdateCalendar(user, teamUpCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.TeamUp));

            DbSync.Synchronize(syncAppointments, synchronizer.Calendars);

            await db.Synchronize(syncAppointments);
        }

    }
   
}

