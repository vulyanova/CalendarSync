using Microsoft.Office.Interop.Outlook;
using Synchronizer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synchronizer.Models;

namespace SyncService.CalendarAdapters
{
    public class OutlookCalendarAdapter: ICalendar
    {
        private readonly Application _oApp;
        private readonly NameSpace _mapiNs;
        private MAPIFolder _calendarFolder;
        private readonly List<AppointmentItem> _items;
        private readonly bool _showSummary ;
        
        public OutlookCalendarAdapter(bool showSummary)
        {
            _oApp = new Application();
            _mapiNs = _oApp.GetNamespace("MAPI");
            _items = new List<AppointmentItem>();
            _showSummary = showSummary;
            var currentFolder = _mapiNs.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);

            _calendarFolder = currentFolder;
        }

        public void ChangeCalendar(string calendar)
        {
            var currentFolder = _mapiNs.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);

            _calendarFolder = currentFolder.Folders[calendar];
        }

        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
            AppointmentItem deleted = null;
            foreach (AppointmentItem item in _items)
            {
                if (item.GlobalAppointmentID == appointment.Id)
                    deleted = item;
            }

            if (deleted!=null)
            {
                _items.Remove(deleted);
                deleted.Delete();
            }
            await Task.CompletedTask;
        }

        public async Task<List<Appointment>> GetNearestAppointmentsAsync()
        {
            var items = _calendarFolder.Items;

            var startDate = DateTime.Now;
            var endDate = startDate.AddMonths(1);

            var list = new List<Appointment>();

            foreach (AppointmentItem item in items)
            {
                if (item.End < startDate || item.Start > endDate) continue;

                _items.Add(item);
                var attendees = new List<string>();
                if (item.RequiredAttendees != null)
                    foreach (var attendee in item.RequiredAttendees.Split(';'))
                        if (attendee.Contains("@"))
                            attendees.Add(attendee.Trim());

                attendees.Sort();

                var newEvent = new Appointment()
                {
                    Id = item.GlobalAppointmentID,
                    Subject = item.Subject,
                    Description = item.Body,
                    Location = item.Location,
                    Date = new AppointmentDate(item.Start, item.End),
                    Updated = item.LastModificationTime,
                    Attendees = attendees
                };
                list.Add(newEvent);
            }

            return await Task.FromResult(list);
        }

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            var attendees = "";
            foreach (var attendee in appointment.Attendees)
            {
                attendees += attendee + "; ";
            }

            foreach (AppointmentItem item in _items)
                if (item.GlobalAppointmentID == appointment.Id)
                {
                    item.Subject = _showSummary ? appointment.Subject : item.Subject;
                    item.Body = _showSummary ? appointment.Description : item.Body;
                    item.Location = appointment.Location;
                    item.Start = appointment.Date.Start;
                    item.End = appointment.Date.End;
                    item.RequiredAttendees = attendees;

                    item.Save();
                    item.Send();
                }

            await Task.CompletedTask;
        }

        public async Task<string> AddAppointmentAsync(Appointment appointment)
        {
            var profile = "";
            _mapiNs.Logon(profile, null, null, null);

            var item = (AppointmentItem)_oApp.CreateItem(OlItemType.olAppointmentItem).Move(_calendarFolder); 

            var attendees = "";
            foreach (var attendee in appointment.Attendees)
                attendees += attendee + "; ";

            item.Subject = _showSummary ? appointment.Subject : "private appointment";
            item.Body = _showSummary ? appointment.Description : "private";
            item.Location = appointment.Location;
            item.Start = appointment.Date.Start;
            item.End = appointment.Date.End;
            item.RequiredAttendees = attendees;
            item.Save();
            item.Send();

            var id = item.GlobalAppointmentID;

            _items.Add(item);

            return await Task.FromResult(id);
        }

    }
}
