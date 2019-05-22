using Calendars;
using Synchronizer;
using SyncService;
using SyncService.CalendarAdapters;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SyncCalendars.Test
{
    public class CalendarsTests
    {
        private static readonly string _clientId = "1064095847198-jolq6914cn32le94k4ksh8m5a1786a27.apps.googleusercontent.com";
        private static readonly string _clientSecret = "AJa_xXPWVdX0V9bZiKJrg3-K";
        private static readonly string _user = "chuvaginavika@gmail.com";
        private static readonly string _accessToken = "ya29.GlsLB7S9TXLIc3MV3JbSKhuMsOk0YEK6q1ZHwHhCpQrl4fzK5XOf6eM_I_ciDUDNFMwUr_imnnz0Z46DUCEAuEqEvamBAJVZztzQjD_ZYBZihp_6MMkXIT1KGB58";
        private static readonly string _refreshToken = "1/D6GIPCOLJBCAWjTE5U92v14GqK9hlKKA_1x7LA7HFFk";
        private static readonly string _calendarId = "9c2pflnloasdoiesmndgi6aej0@group.calendar.google.com";

        private static readonly AuthorizeConfigurations authorizeConfigs = new AuthorizeConfigurations
        {
            ClientId = _clientId,
            ClientSecret = _clientSecret,
            AccessToken = _accessToken,
            RefreshToken = _refreshToken,
            User = _user
        };


        public static ICalendar GetCalendar(int index)
        {
            GoogleCalendarAdapter.Authorize(authorizeConfigs, _calendarId);
            var googleCalendar = GoogleCalendarAdapter.GetInstance();
            var outlookCalendar =OutlookCalendarAdapter.GetInstance();

            var list =  new List<ICalendar>
            {
                googleCalendar,
                outlookCalendar
            };

            return list[index];
    }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task CalendarsAppointments_GetNearestEvents_CorrectListOfEvents(int index)
        {
            var calendar = GetCalendar(index);

            var startDate = DateTime.Now;
            var endDate = startDate.AddMonths(1);

            var appointments = await calendar.GetNearestAppointmentsAsync();

            foreach (var appointment in appointments)
                Assert.InRange(appointment.Date.Start, startDate, endDate);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task CalendarsAppointments_AddEvent_SuccessfullAddition(int index)
        {
            var calendar = GetCalendar(index);
            var subject = "additionTestSubject";

            var startDate = DateTime.Now.AddDays(1);
            var endDate = startDate.AddHours(1);
            
            var appointment = new Appointment()
            {
                Subject = subject,
                Description = "test",
                Date = new AppointmentDate(startDate, endDate),
                Attendees = new List<string>(),
                Location = "test"
            };
            var id = await calendar.AddAppointmentAsync(appointment);
            
            Assert.NotNull(id);

            var appointments = await calendar.GetNearestAppointmentsAsync();

            foreach (var item in appointments)
                if (item.Id == id)
                    await calendar.DeleteAppointmentAsync(item.Id);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async Task CalendarsAppointments_UpdateEventTime_SuccessfullUpdating(int index)
        {
            var calendar = GetCalendar(index);
            var subject = "updatingTestSubject";
            var updatedSubject = subject + "2";

            var startDate = DateTime.Now.AddDays(2);
            var endDate = startDate.AddHours(1);

            var appointments = await calendar.GetNearestAppointmentsAsync();

            var appointment = new Appointment()
            {
                Subject = subject,
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

            await calendar.DeleteAppointmentAsync(id);
        }

    }
}
