using Synchronizer;
using SyncService;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SyncCalendars.Test
{
    public class DbSynchronizerTests
    {
        // todo
        [Fact]
        public void NewAppointments_UpdateDb()
        {
            var existedItems = new List<MainSyncItem>()
            {
                new MainSyncItem()
                {
                    GoogleId = "google1",
                    OutlookId = "outlook1",
                    TeamUpId = "teamUp1"
                },
                new MainSyncItem()
                {
                    GoogleId = "google2",
                    OutlookId = "outlook2",
                    TeamUpId = "teamUp2"
                }
            };

            var googleCalendar = new Calendar
            {
                Appointments = new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.New,
                        Id = "google3",
                        CreatorId = "outlook3"
                    },
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.Checked,
                        Id = "google1"
                    },
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.Checked,
                        Id = "google4"
                    }
                },
                Type = CalendarType.Google
            };

            var outlookCalendar = new Calendar
            {
                Appointments = new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.Checked,
                        Id = "outlook3"
                    },
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.Checked,
                        Id = "outlook1"
                    },
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.New,
                        Id = "outlook4",
                        CreatorId = "google4"
                    }
                },
                Type = CalendarType.Outlook
            };

            var teamUpCalendar = new Calendar
            {
                Appointments = new List<Appointment>
                {
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.New,
                        Id = "teamUp3",
                        CreatorId = "outlook3"
                    },
                    new Appointment
                    {
                        AppointmentStatus = Appointment.Status.Checked,
                        Id = "teamUp1"
                    },
                     new Appointment
                    {
                        AppointmentStatus = Appointment.Status.New,
                        Id = "teamUp4",
                        CreatorId = "google4"
                    }
                },
                Type = CalendarType.TeamUp
            };


            var expected = new List<MainSyncItem>()
            {
                new MainSyncItem()
                {
                    GoogleId = "google1",
                    OutlookId = "outlook1",
                    TeamUpId = "teamUp1"
                },
                new MainSyncItem()
                {
                    GoogleId = "google3",
                    OutlookId = "outlook3",
                    TeamUpId = "teamUp3"
                },
                new MainSyncItem()
                {
                    GoogleId = "google4",
                    OutlookId = "outlook4",
                    TeamUpId = "teamUp4"
                }
            };

            var calendars = new List<Calendar> { googleCalendar, outlookCalendar, teamUpCalendar };

            DbSync.Synchronize(existedItems, calendars);

            Assert.Equal(expected.Count, existedItems.Count);

            foreach (var item in expected)
                Assert.NotEmpty(existedItems.Where(connection =>
                connection.GoogleId == item.GoogleId && connection.OutlookId == item.OutlookId && connection.TeamUpId == item.TeamUpId));
        }

    }
}
