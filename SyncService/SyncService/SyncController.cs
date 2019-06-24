using Calendars;
using SyncService.CalendarAdapters;
using SyncService.DbAdapters.MongoDbAdapter;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Synchronizer.Models;

namespace SyncService
{
    public class SyncController
    {
        private readonly string _url;

        public SyncController()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            _url = config.AppSettings.Settings["Url"].Value;
        }

        private async Task PushLogs(string user, int type, Appointment appointment)
        {
            var client = new HttpClient();
            await client.PostAsJsonAsync(_url + "history/" + user + "/" + type, appointment);
        }

        private async Task<AuthorizeConfigurations> GetAuthorizationConfigurationsAsync(string user)
        {
            AuthorizeConfigurations configs = null;
            var client = new HttpClient();
            var response = await client.GetAsync(_url + "authorize/" + user);
            if (response.IsSuccessStatusCode)
                configs = await response.Content.ReadAsAsync<AuthorizeConfigurations>();

            return configs;
        }

        private async Task UpdateCalendar(string user, ICalendar calendar, Calendar appointmentCalendar)
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

        public async Task Sync(string user)
        {
            var authorizationParams = await GetAuthorizationConfigurationsAsync(user);

            var configuration = Configurations.GetInstance();
            
            var googleCalendar = new GoogleCalendarAdapter(authorizationParams, configuration.CalendarId, !configuration.ShowSummary);
            var outlookCalendar = new OutlookCalendarAdapter(configuration.ShowSummary);
            var teamUpCalendar = new TeamUpCalendarAdapter(authorizationParams.CalendarKey, configuration.TeamUpCalendarId);

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

            var synchronizer = new Synchronizer.Synchronizer(calendars, syncAppointments, !configuration.ShowSummary);

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

