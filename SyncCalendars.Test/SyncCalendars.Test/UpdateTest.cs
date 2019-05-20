using Synchronizer;
using System;
using System.Collections.Generic;

namespace SyncCalendars.Test
{
    public class UpdateTest
    {
        public static List<Appointment> GetGoogleAppointments()
        {
            var googleAppointments = new List<Appointment>();

            googleAppointments.Add(new Appointment
            {
                Id = "google_1",
                Attendees = new List<string> { "chuvaginavika@gmail.com", "vika@gmail.com" },
                Date = new AppointmentDate(new DateTime(2019, 4, 30, 15, 0, 0), new DateTime(2019, 4, 30, 16, 0, 0)),
                Description = "description_1",
                Location = "location_1",
                Subject = "subject_1",
                Updated = new DateTime(2019, 4, 29, 15, 0, 0)
            });

            googleAppointments.Add(new Appointment
            {
                Id = "google_2",
                Attendees = new List<string>(),
                Date = new AppointmentDate(new DateTime(2019, 4, 29, 14, 0, 0), new DateTime(2019, 4, 29, 14, 30, 0)),
                Description = "description_2",
                Location = "location_2",
                Subject = "subject_2",
                Updated = new DateTime(2019, 4, 28, 15, 30, 0)
            });

            return googleAppointments;
        }

        public static List<Appointment> GetOutlookAppointments()
        {
            var outlookAppointments = new List<Appointment>();

            outlookAppointments.Add(new Appointment
            {
                Id = "outlook_1",
                Attendees = new List<string> { "chuvaginavika@gmail.com" },
                Date = new AppointmentDate(new DateTime(2019, 4, 30, 15, 0, 0), new DateTime(2019, 4, 30, 16, 0, 0)),
                Description = "description_1",
                Location = "location_1",
                Subject = "subject_1",
                Updated = new DateTime(2019, 4, 29, 14, 0, 0)
            });

            outlookAppointments.Add(new Appointment
            {
                Id = "outlook_2",
                Attendees = new List<string>(),
                Date = new AppointmentDate(new DateTime(2019, 4, 29, 15, 0, 0), new DateTime(2019, 4, 29, 15, 30, 0)),
                Description = "description_2",
                Location = "location_2",
                Subject = "subject_2",
                Updated = new DateTime(2019, 4, 28, 16, 00, 0)
            });

            return outlookAppointments;
        }

        public static List<MainSyncItem> GetConnection()
        {
            var connection = new List<MainSyncItem>();

            connection.Add(new MainSyncItem
            {
                GoogleId = "google_1",
                OutlookId = "outlook_1"
            });

            connection.Add(new MainSyncItem
            {
                GoogleId = "google_2",
                OutlookId = "outlook_2"
            });

            return connection;
        }

        public static List<Appointment> GetExpectedGoogle()
        {
            var googleAppointments = new List<Appointment>();

            googleAppointments.Add(new Appointment
            {
                Id = "google_1",
                Attendees = new List<string> { "chuvaginavika@gmail.com", "vika@gmail.com" },
                Date = new AppointmentDate(new DateTime(2019, 4, 30, 15, 0, 0), new DateTime(2019, 4, 30, 16, 0, 0)),
                Description = "description_1",
                Location = "location_1",
                Subject = "subject_1",
                Updated = new DateTime(2019, 4, 29, 15, 0, 0),
                AppointmentStatus = Appointment.Status.Checked
            });

            googleAppointments.Add(new Appointment
            {
                Id = "google_2",
                Attendees = new List<string>(),
                Date = new AppointmentDate(new DateTime(2019, 4, 29, 15, 0, 0), new DateTime(2019, 4, 29, 15, 30, 0)),
                Description = "description_2",
                Location = "location_2",
                Subject = "subject_2",
                Updated = new DateTime(2019, 4, 28, 15, 0, 0),
                AppointmentStatus = Appointment.Status.Changed
            });

            return googleAppointments;
        }

        public static List<Appointment> GetExpectedOutlook()
        {
            var outlookAppointments = new List<Appointment>();

            outlookAppointments.Add(new Appointment
            {
                Id = "outlook_1",
                Attendees = new List<string> { "chuvaginavika@gmail.com", "vika@gmail.com" },
                Date = new AppointmentDate(new DateTime(2019, 4, 30, 15, 0, 0), new DateTime(2019, 4, 30, 16, 0, 0)),
                Description = "description_1",
                Location = "location_1",
                Subject = "subject_1",
                Updated = new DateTime(2019, 4, 29, 16, 0, 0)
            });

            outlookAppointments.Add(new Appointment
            {
                Id = "outlook_2",
                Attendees = new List<string>(),
                Date = new AppointmentDate(new DateTime(2019, 4, 29, 15, 0, 0), new DateTime(2019, 4, 29, 15, 30, 0)),
                Description = "description_2",
                Location = "location_2",
                Subject = "subject_2",
                Updated = new DateTime(2019, 4, 28, 15, 00, 0)
            });

            return outlookAppointments;
        }
    }
}
