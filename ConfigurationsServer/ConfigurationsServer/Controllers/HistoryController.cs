using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Synchronizer;
using System.Collections.Generic;
using System.Linq;

namespace ConfigurationsServer.Controllers
{
    [Route("api/[controller]/")]
    public class HistoryController : Controller
    {
        /*[EnableCors("Policy")]
        [HttpGet("{user}")]
        public UICalendar[] GetStep(string user)
        {
           *var firstPrivious = new UIAppointment { Subject = "subject1", Description = "description1", Attendees = "attendees1", Date = "01/01/2012 10:30-01/01/2012 11:30" };
            var firstPresent = new UIAppointment { Subject = "subject1", Description = "new description of the first appointment (very long description)", Attendees = "attendees1", Date = "01/01/2012 10:30-01/01/2012 11:30" };

            var secondPrevious = new UIAppointment { Subject = "subject2", Description = "description2", Attendees = "attendees2", Date = "01/01/2012 13:30-01/01/2012 14:30" };
            var secondPresent = new UIAppointment { Subject = "DELETED", Description = "", Attendees = "", Date = "" };

            var thirdPrevious = new UIAppointment { Subject = "subject3", Description = "description3", Attendees = "attendees3", Date = "01/01/2012 16:30-01/01/2012 17:30" };
            var thirdPresent = new UIAppointment { Subject = "subject3", Description = "description3", Attendees = "attendees3", Date = "01/01/2012 16:30-01/01/2012 17:30" };

            var cleanAppointment = new UIAppointment { Subject = "", Description = "", Attendees = "", Date = "" };


            var calendar1 = new UICalendar {
                Name = "Google",
                Previous = new UIAppointment[] { firstPrivious, secondPrevious, thirdPrevious },
                Present = new UIAppointment[] { firstPresent, thirdPresent }
            };

            var calendar2 = new UICalendar
            {
                Name = "Outlook",
                Previous = new UIAppointment[] { firstPresent },
                Present = new UIAppointment[] { firstPresent, thirdPresent }
            };

            var calendar3 = new UICalendar
            {
                Name = "TeamUp",
                Previous = new UIAppointment[] { firstPrivious, secondPrevious },
                Present = new UIAppointment[] { firstPresent, thirdPresent }
            };

            return GetCalendar();
        }

        private static UICalendar[] GetCalendar()
        {
            var client = new MongoClient("mongodb+srv://newadmin:02072012@calendarcluster-tjsbr.gcp.mongodb.net/test?retryWrites=true");
            var database = client.GetDatabase("SyncConfigurations");
            var collection = database.GetCollection<Synchronizer.Calendar>("Calendars").AsQueryable();

            var UICalendars = new List<UICalendar>();
            foreach (var calendar in collection)
                UICalendars.Add(CreateUICalendar(calendar.Appointments, calendar.Type));

            return UICalendars.ToArray();
        }

        private static UICalendar CreateUICalendar(List<Appointment> appointments, CalendarType type)
        {
            string name = "Calendar";

            switch (type)
            {
                case CalendarType.Google:
                    name = "Google";
                    break;
                case CalendarType.Outlook:
                    name = "Outlook";
                    break;
                case CalendarType.TeamUp:
                    name = "TeamUp";
                    break;
            }

            var calendar = new UICalendar
            {
                Name = name,
                Previous = CreatePreviousState(appointments),
                Present = CreatePresentState(appointments),
            };

            return calendar;
        }

        private static UIAppointment[] CreatePreviousState(List<Appointment> appointments)
        {
            var previous = new List<UIAppointment>();

            foreach (var appointment in appointments)
            {
                if (appointment.AppointmentStatus == Appointment.Status.Changed)
                {
                    var previousAppointment = new UIAppointment
                    {
                        Subject = appointment.PreviousSubject,
                        Description = appointment.PreviousDescription,
                        Date = appointment.PreviousDate.Start.ToString("g") + " - " + appointment.PreviousDate.End.ToString("g"),
                        Attendees = string.Join(", ", appointment.PreviousAttendees.ToArray())
                    };
                    previous.Add(previousAppointment);
                }
                else if (appointment.AppointmentStatus != Appointment.Status.New)
                {
                    var previousAppointment = new UIAppointment
                    {
                        Subject = appointment.Subject,
                        Description = appointment.Description,
                        Date = appointment.Date.Start.ToString("g") + " - " + appointment.Date.End.ToString("g"),
                        Attendees = string.Join(", ", appointment.Attendees.ToArray())
                    };
                    previous.Add(previousAppointment);
                }
            }
            return previous.OrderBy(item => item.Date).ToArray();
        }

        private static UIAppointment[] CreatePresentState(List<Appointment> appointments)
        {
            var cleanAppointment = new UIAppointment { Subject = "", Description = "", Attendees = "", Date = "" };
            var present = new List<UIAppointment>();

            foreach (var appointment in appointments)
            {
                if (appointment.AppointmentStatus != Appointment.Status.Deleted)
                {
                    var presentAppointment = new UIAppointment
                    {
                        Subject = appointment.Subject,
                        Description = appointment.Description,
                        Date = appointment.Date.Start.ToString("g") + " - " + appointment.Date.End.ToString("g"),
                        Attendees = string.Join(", ", appointment.Attendees.ToArray())
                    };
                    present.Add(presentAppointment);
                }
            }
            return present.OrderBy(item => item.Date).ToArray();
        }*/

    }
}