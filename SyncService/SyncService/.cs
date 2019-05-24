using Synchronizer;
using SyncService.DbAdapters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static async Task Sync(IDbInteraction db, List<Calendar> calendars)
        {
            var items = await db.GetCalendarItems();
            foreach (var item in items)
                if (calendars.Where(calendar => calendar.Type == CalendarType.Google && calendar.Appointments.Where(appointment => appointment.Id == item.GoogleId).Count() == 1).Count() == 0)
                    await db.Remove(item);

            foreach (var calendar in calendars)
            {
                foreach (var appointment in calendar.Appointments)
                    if (appointment.AppointmentStatus == Appointment.Status.New)
                        await db.Add(CreateAppointment(appointment, calendar.Type));
            }

            await db.Save();

        }
    }
}
