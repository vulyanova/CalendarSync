using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Calendars.TeamUpCalendar
{
    public class TeamUpCalendar
    {
        private readonly string _apiKey = "0ad07f8905ca44f73a62048fcf3aaf7c485dec5c036d5647806daa4bb6157b94";
        public string CalendarKey;
        private readonly HttpClient _client = new HttpClient();
        public readonly string Url = "https://api.teamup.com/";

        public TeamUpCalendar(string calendarKey)
        {
            CalendarKey = calendarKey;
        }

        public async Task<List<Calendar>> GetCalendars()
        {
            var webRequest = WebRequest.Create(Url + CalendarKey + "/subcalendars");

            webRequest.Method = "GET";
            webRequest.Timeout = 12000;
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add("Teamup-Token", _apiKey);

            var response = await webRequest.GetResponseAsync();
            var s = response.GetResponseStream();
            var sr = new System.IO.StreamReader(s);

            var jsonResponse = sr.ReadToEnd();
            dynamic des = JsonConvert.DeserializeObject(jsonResponse);
            var list = new List<Calendar>();
            foreach (var appointment in des.subcalendars)
            {
                var calendar = new Calendar()
                {
                    Id = appointment.id,
                    Name = appointment.name
                };

                list.Add(calendar);
            }

            return list;

        }

        public async Task AddAppointment(int calendarId, TeamUpEvent appointment)
        {
            dynamic data = new
            {
                title = appointment.Title,
                location = appointment.Location,
                notes = appointment.Description,
                who = appointment.Who,
                start_dt = appointment.Start.ToString("yyyy-MM-ddTHH:mm:ssK"),
                end_dt = appointment.End.ToString("yyyy-MM-ddTHH:mm:ssK"),
                subcalendar_id = calendarId
            };

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(Url + CalendarKey + "/events"),
                Headers =
                {
                    {"Teamup-Token", _apiKey}
                },
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };

            var response = _client.SendAsync(httpRequestMessage).Result;
            var result = await response.Content.ReadAsStringAsync();

            dynamic json = JsonConvert.DeserializeObject(result);
            appointment.Id = json.@event.id;
            appointment.Version = json.@event.version;
        }

        public async Task UpdateAppointment(int calendarId, TeamUpEvent appointment)
        {
            dynamic data = new
            {
                id = appointment.Id,
                title = appointment.Title,
                location = appointment.Location,
                notes = appointment.Description,
                who = appointment.Who,
                version = appointment.Version,
                start_dt = appointment.Start.ToString("yyyy-MM-ddTHH:mm:ssK"),
                end_dt = appointment.End.ToString("yyyy-MM-ddTHH:mm:ssK"),
                subcalendar_id = calendarId
            };

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(Url + CalendarKey + "/events/" + appointment.Id),
                Headers =
                {
                    {"Teamup-Token", _apiKey}
                },
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };

            var response = _client.SendAsync(httpRequestMessage).Result;
            var result = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(result);
            appointment.Version = json.@event.version;
        }

        public async Task DeleteAppointment(int calendarId, TeamUpEvent appointment)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(Url + CalendarKey + "/events/" + appointment.Id + "?version=" +
                                     appointment.Version + "&redit=all"),
                Headers =
                {
                    {"Teamup-Token", _apiKey}
                }
            };

            var response = _client.SendAsync(httpRequestMessage).Result;
            await response.Content.ReadAsStringAsync();
        }

        public async Task<List<TeamUpEvent>> GetNearestAppointments(int calendarId)
        {
            var startDate = "startDate=" + DateTime.Now.ToShortDateString();
            var endDate = "endDate=" + DateTime.Now.AddMonths(1).ToShortDateString();
            var webRequest = WebRequest.Create(Url + CalendarKey + "/events?" + startDate + "&" + endDate);

            webRequest.Method = "GET";
            webRequest.Timeout = 12000;
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add("Teamup-Token", _apiKey);

            var response = await webRequest.GetResponseAsync();
            var s = response.GetResponseStream();
            var sr = new System.IO.StreamReader(s);

            var jsonResponse = sr.ReadToEnd();
            dynamic des = JsonConvert.DeserializeObject(jsonResponse);
            var list = new List<TeamUpEvent>();
            foreach (var appointment in des.events)
            {
                if (appointment.subcalendar_id == calendarId)
                {
                    var teamUpEvent = new TeamUpEvent()
                    {
                        Id = appointment.id,
                        Location = appointment.location,
                        Description = appointment.notes,
                        Who = appointment.who,
                        Start = (DateTime) appointment.start_dt,
                        End = (DateTime) appointment.end_dt,
                        Version = appointment.version,
                        Title = appointment.title,
                        Update = appointment.update_dt != null
                            ? (DateTime) appointment.update_dt
                            : (DateTime) appointment.creation_dt
                    };

                    list.Add(teamUpEvent);
                }
            }

            return list;
        }
    }
}
