using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace Calendars.GoogleCalendar
{
    public class GoogleCalendar
    {
        public UserCredential Credential { get; private set; }
        public CalendarService CalendarService { get; private set; }

        private void AuthorizeWithTokens(AuthorizeConfigurations authorizeConfigs)
        {
            ClientSecrets secrets = new ClientSecrets()
            {
                ClientId = authorizeConfigs.ClientId,
                ClientSecret = authorizeConfigs.ClientSecret
            };

            var token = new TokenResponse
            {
                AccessToken = authorizeConfigs.AccessToken,
                RefreshToken = authorizeConfigs.RefreshToken
            };

            Credential = new UserCredential(new GoogleAuthorizationCodeFlow(
                 new GoogleAuthorizationCodeFlow.Initializer
                 {
                     ClientSecrets = secrets
                 }),
                 authorizeConfigs.User,
                 token);
        }

        private void AuthorizeWithoutTokens(AuthorizeConfigurations authorizeConfigs)
        {
            var scopes = new string[] { CalendarService.Scope.Calendar };

            Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                 new ClientSecrets
                 {
                     ClientId = authorizeConfigs.ClientId,
                     ClientSecret = authorizeConfigs.ClientSecret
                 },
                scopes,
                authorizeConfigs.User,
                CancellationToken.None).Result;
        }

        public GoogleCalendar(AuthorizeConfigurations authorizeConfigs)
        {
            if (authorizeConfigs.AccessToken == null || authorizeConfigs.AccessToken == "")
                AuthorizeWithoutTokens(authorizeConfigs);
            else AuthorizeWithTokens(authorizeConfigs);
        }

        public async Task<List<Calendar>> GetCalendars()
        {
            var service = GetService();

            var calendars = await service.CalendarList.List().ExecuteAsync();

            var calendarList = new List<Calendar>();

            foreach (var calendar in calendars.Items)
            {
                calendarList.Add(new Calendar()
                {
                    Id = calendar.Id,
                    Name = calendar.Summary
                });
            }
            return calendarList;
        }

        public CalendarService GetService()
        {
            CalendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
            });

            return CalendarService;
        }


        public async Task DeleteAppointment(string calendarId, string id)
        {
            await CalendarService.Events.Delete(calendarId, id).ExecuteAsync();
        }

        public async Task UpdateAppointment(string calendarId, Event googleEvent)
        {
            await CalendarService.Events.Update(googleEvent, calendarId, googleEvent.Id).ExecuteAsync();
        }

        public async Task<Event> GetAppointment(string calendarId, string appointmentId)
        {
            return await CalendarService.Events.Get(calendarId, appointmentId).ExecuteAsync();
        }

        public async Task<string> AddAppointment(string calendarId, Event googleEvent)
        {
            var createdEvent = await CalendarService.Events.Insert(googleEvent, calendarId).ExecuteAsync();

            return createdEvent.Id;
        }

        public async Task<Events> GetNearestAppointmentsAsync(string calendarId)
        {
            var request = CalendarService.Events.List(calendarId);
            request.TimeMin = DateTime.Now.ToLocalTime();
            request.TimeMax = DateTime.Now.AddMonths(1).ToLocalTime();
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            return await request.ExecuteAsync();
        }

    }
}
