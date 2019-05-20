using Synchronizer;
using SyncService.DbAdapters;
using System.Collections.Generic;
using System.Linq;

namespace SyncService
{
    public static class DbSync
    {
        private static MainSyncItem CreateAppointment(Appointment appointment, CalendarType type)
        {
            var item = new MainSyncItem();

            if (type == CalendarType.Google)
                item = new MainSyncItem
                {
                    GoogleId = appointment.Id,
                    OutlookId = appointment.CreatorId
                };

            if (type == CalendarType.Outlook)
                item = new MainSyncItem
                {
                    GoogleId = appointment.CreatorId,
                    OutlookId = appointment.Id
                };

            return item;
        }

        public static void Sync(IDbInteraction db, List<Calendar> calendars)
        {
            foreach (var item in db.GetCalendarItems())
                if (calendars.Where(calendar => calendar.Type == CalendarType.Google && calendar.Appointments.Where(appointment => appointment.Id == item.GoogleId).Count() == 1).Count() == 0)
                    db.Remove(item);

            foreach (var calendar in calendars)
            {
                foreach (var appointment in calendar.Appointments)
                    if (appointment.AppointmentStatus == Appointment.Status.New)
                        db.Add(CreateAppointment(appointment, calendar.Type));
            }

            db.Save();

        }
    }
}
