using Synchronizer;
using System;
using System.Collections.Generic;

namespace SyncCalendars.Test
{
    public static class ItemsCollection
    {
        public static Calendar GetGoogleAppointments()
        {
            var googleAppointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = "google_1",
                    Attendees = new List<string> { "chuvaginavika@gmail.com", "vika@gmail.com" },
                    Date = new AppointmentDate(new DateTime(2019, 4, 30, 15, 0, 0), new DateTime(2019, 4, 30, 16, 0, 0)),
                    Description = "description_1",
                    Location = "location_1",
                    Subject = "subject_1",
                    Updated = new DateTime(2019, 4, 29, 15, 0, 0)
                },

                new Appointment
                {
                    Id = "google_2",
                    Attendees = new List<string>(),
                    Date = new AppointmentDate(new DateTime(2019, 4, 29, 15, 0, 0), new DateTime(2019, 4, 29, 15, 30, 0)),
                    Description = "changedDescription_2",
                    Location = "location_2",
                    Subject = "subject_2",
                    Updated = new DateTime(2019, 4, 28, 15, 30, 0)
                }
            };

            var calendar = new Calendar
            {
                Appointments = googleAppointments,
                Type = CalendarType.Google
            };

            return calendar;
        }

        public static Calendar GetOutlookAppointments()
        {
            var outlookAppointments = new List<Appointment>
            {
                new Appointment
                {
                    Id = "outlook_1",
                    Attendees = new List<string> { "chuvaginavika@gmail.com" },
                    Date = new AppointmentDate(new DateTime(2019, 4, 30, 15, 0, 0), new DateTime(2019, 4, 30, 16, 0, 0)),
                    Description = "description_1",
                    Location = "location_1",
                    Subject = "subject_1",
                    Updated = new DateTime(2019, 4, 29, 14, 0, 0)
                },

                new Appointment
                {
                    Id = "outlook_2",
                    Attendees = new List<string>(),
                    Date = new AppointmentDate(new DateTime(2019, 4, 29, 15, 0, 0), new DateTime(2019, 4, 29, 15, 30, 0)),
                    Description = "description_2",
                    Location = "location_2",
                    Subject = "subject_2",
                    Updated = new DateTime(2019, 4, 28, 16, 00, 0)
                }
            };
            var calendar = new Calendar
            {
                Appointments = outlookAppointments,
                Type = CalendarType.Outlook
            };

            return calendar;
        }
    }
}
