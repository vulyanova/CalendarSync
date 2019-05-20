using Synchronizer;
using System.Collections.Generic;
using Xunit;

namespace SyncCalendars.Test
{
    public class SynchronizerTest
    {
        public static Synchronizer.Synchronizer SyncConnectedCalendars()
        {
            var googleAppointments = UpdateTest.GetGoogleAppointments();
            var outlookAppointments = UpdateTest.GetOutlookAppointments();
            var connection = UpdateTest.GetConnection();

            var googleItems = new Calendar
            {
                Appointments = googleAppointments,
                Type = CalendarType.Google
            };

            var outlookItems = new Calendar
            {
                Appointments = outlookAppointments,
                Type = CalendarType.Outlook
            };

            var synchronizer = new Synchronizer.Synchronizer(new List<Calendar> { googleItems, outlookItems }, connection, false);

            synchronizer.SyncExistedAppointments();
            synchronizer.AddNewAppointments();

            return synchronizer;
        }

        public static Synchronizer.Synchronizer SyncSimpleCalendars()
        {
            var googleAppointments = UpdateTest.GetGoogleAppointments();
            var outlookAppointments = UpdateTest.GetOutlookAppointments();
            var connection = new List<MainSyncItem>();

            var googleItems = new Calendar
            {
                Appointments = googleAppointments,
                Type = CalendarType.Google
            };

            var outlookItems = new Calendar
            {
                Appointments = outlookAppointments,
                Type = CalendarType.Outlook
            };

            var synchronizer = new Synchronizer.Synchronizer(new List<Calendar> { googleItems, outlookItems }, connection, false);

            synchronizer.SyncExistedAppointments();
            synchronizer.AddNewAppointments();

            return synchronizer;
        }

        /*[Fact]
        public void NewAppointments_AppointmentStatusNew()
        {
            var expected = Appointment.Status.New;

            var synchronizer = SyncSimpleCalendars();

            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == CalendarType.Google);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Subject == "outlook subject");

            Assert.Equal(expected,newGoogleItem.AppointmentStatus);
        }

        [Fact]
        public void NewAppointments_AppointmentStatusChecked()
        {
            var expected = Appointment.Status.Checked;

            var synchronizer = SyncSimpleCalendars();

            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == CalendarType.Google);
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Subject == "google subject");

            Assert.Equal(expected, newGoogleItem.AppointmentStatus);
        }

        [Fact]
        public void NewAppointments_SetCreatorId()
        {
            var expected = "outlook1";

            var synchronizer = SyncSimpleCalendars();

            var googleCalendar = synchronizer.Calendars.Find(item => item.Type == CalendarType.Google);      
            var newGoogleItem = googleCalendar.Appointments.Find(item => item.Subject == "outlook subject");

            Assert.Equal(expected, newGoogleItem.CreatorId);
        }*/

        [Fact]
        public void NewAppointments_AddAppointments()
        {
            var synchronizer = SyncSimpleCalendars();

            foreach (var calendar in synchronizer.Calendars)
            {
                Assert.Equal(4, calendar.Appointments.Count);
            }
        }

        [Fact]
        public void ExistedAppointments_UpdatedAppointments()
        {
            var synchronizer = SyncConnectedCalendars();

            var googleAppointments = synchronizer.Calendars.Find(item => item.Type == CalendarType.Google).Appointments;
            var expectedGoogleAppointments = UpdateTest.GetExpectedGoogle();

            foreach (var appointment in googleAppointments)
            {
                var expectedAppointment = expectedGoogleAppointments.Find(item => item.Id == appointment.Id);

                Assert.Equal(expectedAppointment.Date.Start, appointment.Date.Start);
                Assert.Equal(expectedAppointment.Location, appointment.Location);
                Assert.Equal(expectedAppointment.Attendees, appointment.Attendees);
            }

            var outlookAppointments = synchronizer.Calendars.Find(item => item.Type == CalendarType.Outlook).Appointments;
            var expectedOutlookAppointments = UpdateTest.GetExpectedOutlook();

            foreach (var appointment in outlookAppointments)
            {
                var expectedAppointment = expectedOutlookAppointments.Find(item => item.Id == appointment.Id);

                Assert.Equal(expectedAppointment.Date.Start, appointment.Date.Start);
                Assert.Equal(expectedAppointment.Location, appointment.Location);
                Assert.Equal(expectedAppointment.Attendees, appointment.Attendees);
            }
        }
    }
}
