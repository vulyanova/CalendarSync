using Calendars;
using MongoDB.Driver;
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
        private static IMongoCollection<Log> _logCollection;

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

                var log = new Log(user, item.AppointmentStatus, appointmentCalendar.Type, item);

                await _logCollection.InsertOneAsync(log);
            }
        }

        public static async Task Sync(string user, IMongoCollection<Log> logCollection)
        {
            _logCollection = logCollection;
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

            await UpdateCalendar(user, googleCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.Google));
            await UpdateCalendar(user, outlookCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.Outlook));
            await UpdateCalendar(user, teamUpCalendar, synchronizer.Calendars.Find(item => item.Type == CalendarType.TeamUp));

            DbSync.Synchronize(syncAppointments, synchronizer.Calendars);

            await db.Synchronize(syncAppointments);
        }

    }
   
}

