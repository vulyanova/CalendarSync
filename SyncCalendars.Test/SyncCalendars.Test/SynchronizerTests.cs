using Synchronizer;
using System.Collections.Generic;
using Synchronizer.Models;
using Xunit;

namespace SyncCalendars.Test
{
    public class SynchronizerTests
    {
        public static Synchronizer.Synchronizer SyncCalendars(List<MainSyncItem> connection, bool isPrivate)
        {
            var googleCalendar = ItemsCollection.GetGoogleAppointments();
            var outlookCalendar = ItemsCollection.GetOutlookAppointments();

            var synchronizer = new Synchronizer.Synchronizer(new List<Calendar> { googleCalendar, outlookCalendar }, connection, isPrivate);

            synchronizer.SyncExistedAppointments();
            synchronizer.AddNewAppointments();

            return synchronizer;
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_1")]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Outlook, "outlook_1")]
        [InlineData(CalendarType.Outlook, "outlook_2")]
        public void NewAppointments_AppointmentStatusChecked(CalendarType type, string id)
        {
            var expected = Appointment.Status.Checked;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForAdding(), false);
            var calendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newItem = calendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "subject_1", "outlook_1")]
        [InlineData(CalendarType.Google, "subject_2", "outlook_2")]
        [InlineData(CalendarType.Outlook, "subject_1", "google_1")]
        [InlineData(CalendarType.Outlook, "subject_2", "google_2")]
        public void NewAppointments_CorrectCreatorId(CalendarType type, string subject, string expected)
        {
            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForAdding(), false);
            var calendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newItem = calendar.Appointments.
                Find(item => item.Subject == subject && item.Id == null);

            Assert.Equal(expected, newItem.CreatorId);
        }

        [Theory]
        [InlineData(CalendarType.Google, "outlook_1")]
        [InlineData(CalendarType.Google, "outlook_2")]
        [InlineData(CalendarType.Outlook, "google_1")]
        [InlineData(CalendarType.Outlook, "google_2")]
        public void NewAppointments_AppointmentStatusNew(CalendarType type, string creatorId)
        {
            var expected = Appointment.Status.New;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForAdding(), false);
            var calendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newItem = calendar.Appointments.Find(item => item.CreatorId == creatorId);

            Assert.Equal(expected, newItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Outlook, "outlook_1")]
        public void UpdateAppointments_AppointmentStatusChanged(CalendarType type, string id)
        {
            var expected = Appointment.Status.Changed;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForUpdating(), false);
            var calendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newItem = calendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Outlook, "outlook_2")]
        public void DeletedAppointments_AppointmentStatusDeleted(CalendarType type, string id)
        {
            var expected = Appointment.Status.Deleted;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForDeleting(), false);
            var calendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newItem = calendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Google, "google_1")]
        [InlineData(CalendarType.Outlook, "outlook_2")]
        public void UpdatedPrivateAppointments_AppointmentStatusChecked(CalendarType type, string id)
        {
            var expected = Appointment.Status.Checked;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForUpdating(), true);
            var calendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newItem = calendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Google, "google_1")]
        [InlineData(CalendarType.Outlook, "outlook_2")]
        public void ThreeCalendarsSynchronization_AddAppointment_SuccessfulAddition(CalendarType type, string id)
        {
            var connection = new List<MainSyncItem>();

            connection.Add(new MainSyncItem
            {
                GoogleId = "google_1",
                OutlookId = "outlook_1",
                TeamUpId = "teamUp_1"
            });

            connection.Add(new MainSyncItem
            {
                GoogleId = "google_2",
                OutlookId = "outlook_2",
                TeamUpId = "teamUp_2"
            });

            var googleCalendar = ItemsCollection.GetGoogleAppointments();
            var outlookCalendar = ItemsCollection.GetOutlookAppointments();
            var teamUpCalendar = ItemsCollection.GetTeamUpAppointments();

            var calendars = new List<Calendar> { googleCalendar, outlookCalendar, teamUpCalendar };

            var synchronizer = new Synchronizer.Synchronizer(calendars, connection, false);
            synchronizer.SyncExistedAppointments();
            synchronizer.AddNewAppointments();

            Assert.Equal("1","1");
        }

        [Fact]
        public void NewAppointments_AddCorrectAmount()
        {
            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForAdding(), false);

            foreach (var calendar in synchronizer.Calendars)
                Assert.Equal(4, calendar.Appointments.Count);       
        }
    }
}
