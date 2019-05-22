using Microsoft.Office.Interop.Outlook;
using Synchronizer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncService.CalendarAdapters
{
    public class OutlookCalendarAdapter: ICalendar
    {
        private static OutlookCalendarAdapter _instance;
        private Application _oApp;
        private NameSpace _mapiNS;
        private List<AppointmentItem> _items;
        private bool _showSummary ;

        private OutlookCalendarAdapter()
        {
            _oApp = new Application();
            _mapiNS = _oApp.GetNamespace("MAPI");
            _items = new List<AppointmentItem>();
            _showSummary = true;
        }

        public void ShowSummary()
        {
            _showSummary = true;
        }

        public void HideSummary()
        {
            _showSummary = false;
        }

        public static OutlookCalendarAdapter GetInstance()
        {
            if (_instance == null)
                _instance = new OutlookCalendarAdapter();

            return _instance;
        }

        public async Task DeleteAppointmentAsync(string id)
        {
            foreach (AppointmentItem item in _items)
            {
                if (item.GlobalAppointmentID == id)
                    item.Delete();
            }
            await Task.CompletedTask;
        }

        public async Task<List<Appointment>> GetNearestAppointmentsAsync()
        {
            var CalendarFolder = _mapiNS.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);
            var items = CalendarFolder.Items;

            var startDate = DateTime.Now;
            var endDate = startDate.AddMonths(1);

            var list = new List<Appointment>();

            foreach (AppointmentItem item in items)
            {
                if (item.End >= startDate && item.Start <= endDate)
                {
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
            string profile = "";
            _mapiNS.Logon(profile, null, null, null);

            var item = (_AppointmentItem)_oApp.CreateItem(OlItemType.olAppointmentItem);

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

            return await Task.FromResult(id);
        }

        public void Disconnect()
        {
            _oApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_mapiNS);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_oApp);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(_items);

            _instance = null;
        }

        public async Task UpdateAsync(List<Appointment> appointments)
        {
            foreach (var item in appointments)
            {
                if (item.AppointmentStatus == Appointment.Status.New)
                    item.Id = await AddAppointmentAsync(item);

                if (item.AppointmentStatus == Appointment.Status.Deleted)
                    await DeleteAppointmentAsync(item.Id);

                if (item.AppointmentStatus == Appointment.Status.Changed)
                    await UpdateAppointmentAsync(item);
            }

            await Task.CompletedTask;
        }
    }
}
