using Calendars;
using SyncService.CalendarAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Synchronizer.Models;
using Xunit;

namespace SyncCalendars.Test
{
    public class CalendarsTests
    {
        private const string ClientId = "1064095847198-jolq6914cn32le94k4ksh8m5a1786a27.apps.googleusercontent.com";
        private const string ClientSecret = "AJa_xXPWVdX0V9bZiKJrg3-K";
        private const string User = "chuvaginavika@gmail.com";
        private const string AccessToken = "ya29.GlsLB7S9TXLIc3MV3JbSKhuMsOk0YEK6q1ZHwHhCpQrl4fzK5XOf6eM_I_ciDUDNFMwUr_imnnz0Z46DUCEAuEqEvamBAJVZztzQjD_ZYBZihp_6MMkXIT1KGB58";
        private const string RefreshToken = "1/D6GIPCOLJBCAWjTE5U92v14GqK9hlKKA_1x7LA7HFFk";
        private const string CalendarId = "l9b4t4broehnafqhemo2b264ho@group.calendar.google.com";
        private const string Subject = "calendarTest";
        private const string OutlookCalendarName = "calendar for tests";

        private static readonly AuthorizeConfigurations AuthorizeConfigs = new AuthorizeConfigurations
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            AccessToken = AccessToken,
            RefreshToken = RefreshToken,
            User = User
        };

        public static ICalendar GetCalendar(int index)
        {
            GoogleCalendarAdapter.Authorize(AuthorizeConfigs, CalendarId, true);
            var googleCalendar = GoogleCalendarAdapter.GetInstance();
            var outlookCalendar =OutlookCalendarAdapter.GetInstance();
            var teamUpCalendar = new TeamUpCalendarAdapter("ksjea1t78n1525ka23", 6551483);
            outlookCalendar.ChangeCalendar(OutlookCalendarName);

            var list =  new List<ICalendar>
            {
                googleCalendar,
                outlookCalendar,
                teamUpCalendar
            };

            return list[index];
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task CalendarsAppointments_GetNearestEvents_CorrectRangeOfEvents(int index)
        {
            var calendar = GetCalendar(index);

            var startDate = DateTime.Now;
            var endDate = startDate.AddMonths(1);

            var appointments = await calendar.GetNearestAppointmentsAsync();

            foreach (var appointment in appointments)
                Assert.InRange(appointment.Date.End, startDate, endDate);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task CalendarsAppointments_AddEvent_SuccessfullAddition(int index)
        {
            var calendar = GetCalendar(index);

            var firstAttendee = "chuvaginavika@gmail.com";
            var secondAttendee = "chuvaginavika@icloud.com";

            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddHours(1);
            
            var appointment = new Appointment()
            {
                Subject = Subject,
                Description = "test",
                Date = new AppointmentDate(startDate, endDate),
                Attendees = new List<string> {secondAttendee, firstAttendee },
                Location = "test"
            };

            var id = await calendar.AddAppointmentAsync(appointment);

            var appointments = await calendar.GetNearestAppointmentsAsync();
            var newApp = appointments.Where(item =>item.Id==id && item.Attendees[0] == firstAttendee && item.Attendees[1] == secondAttendee);

            Assert.Single(newApp);       
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task CalendarsAppointments_UpdateEventTime_SuccessfullUpdating(int index)
        {
            var calendar = GetCalendar(index);

            var startDate = DateTime.Now.AddDays(2);
            var endDate = startDate.AddHours(1);

            var appointments = await calendar.GetNearestAppointmentsAsync();

            var appointment = new Appointment()
            {
                Subject = Subject,
                Description = "test",
                Date = new AppointmentDate(startDate, endDate),
                Attendees = new List<string>(),
                Location = "test"
            };
            var id = await calendar.AddAppointmentAsync(appointment);

            startDate.AddHours(1);
            endDate.AddHours(1);

            appointment.Id = id;
            appointment.Date = new AppointmentDate(startDate, endDate);

            await calendar.UpdateAppointmentAsync(appointment);

            appointments = await calendar.GetNearestAppointmentsAsync();
            var updatedAppointment = appointments.Find(item => item.Id == id);
    
            Assert.Equal(startDate, updatedAppointment.Date.Start, TimeSpan.FromMinutes(1));

        }

    }
}
