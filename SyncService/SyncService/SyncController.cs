using Calendars;
using Synchronizer;
using SyncService.CalendarAdapters;
using SyncService.DbAdapters.MongoDbAdapter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                try
                {
                    configs = await response.Content.ReadAsAsync<AuthorizeConfigurations>();
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("SyncService", "Invalid authorization request response format: "+ ex.Message);
                }
            }
            else
                EventLog.WriteEntry("SyncService", "Authorization request failed:" + response.StatusCode);

            return configs;
        }

        public static async Task UpdateCalendar(ICalendar calendar, List<Appointment> appointments)
        {
            foreach (var item in appointments)
            {
                if (item.AppointmentStatus == Appointment.Status.New)
                    item.Id = await calendar.AddAppointmentAsync(item);

                if (item.AppointmentStatus == Appointment.Status.Deleted)
                    await calendar.DeleteAppointmentAsync(item);

                if (item.AppointmentStatus == Appointment.Status.Changed)
                    await calendar.UpdateAppointmentAsync(item);
            }
        }

        public static async Task Sync(string user)
        {
            var authorizationParams = await GetAuthorizationConfigurationsAsync(url+user);

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

            await UpdateCalendar(googleCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.Google).Appointments);
            await UpdateCalendar(outlookCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.Outlook).Appointments);
            await UpdateCalendar(teamUpCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.TeamUp).Appointments);

            DbSync.Synchronize(syncAppointments, synchronizer.Calendars);

            await db.Synchronize(syncAppointments);
        }

    }
   
}

