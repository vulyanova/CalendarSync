using Synchronizer;
using System.Collections.Generic;
using Xunit;

namespace SyncCalendars.Test
{
    public class SynchronizerTest
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
            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newGoogleItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "subject_1", "outlook_1")]
        [InlineData(CalendarType.Google, "subject_2", "outlook_2")]
        [InlineData(CalendarType.Outlook, "subject_1", "google_1")]
        [InlineData(CalendarType.Outlook, "subject_2", "google_2")]
        public void NewAppointments_CorrectCreatorId(CalendarType type, string subject, string expected)
        {
            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForAdding(), false);
            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newGoogleItem = googleCalendar.Appointments.
                Find(item => item.Subject == subject && item.Id == null);

            Assert.Equal(expected, newGoogleItem.CreatorId);
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
            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.CreatorId == creatorId);

            Assert.Equal(expected, newGoogleItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Outlook, "outlook_1")]
        public void UpdateAppointments_AppointmentStatusChanged(CalendarType type, string id)
        {
            var expected = Appointment.Status.Changed;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForUpdating(), false);
            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newGoogleItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Outlook, "outlook_2")]
        public void DeletedAppointments_AppointmentStatusDeleted(CalendarType type, string id)
        {
            var expected = Appointment.Status.Deleted;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForDeleting(), false);
            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newGoogleItem.AppointmentStatus);
        }

        [Theory]
        [InlineData(CalendarType.Google, "google_2")]
        [InlineData(CalendarType.Google, "google_1")]
        [InlineData(CalendarType.Outlook, "outlook_2")]
        public void UpdatedPrivateAppointments_AppointmentStatusChecked(CalendarType type, string id)
        {
            var expected = Appointment.Status.Checked;

            var synchronizer = SyncCalendars(ConnectionsCollection.GetConnectionForUpdating(), true);
            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == type);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Id == id);

            Assert.Equal(expected, newGoogleItem.AppointmentStatus);
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
