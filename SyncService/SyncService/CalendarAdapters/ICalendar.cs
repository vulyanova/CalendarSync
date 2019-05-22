using Synchronizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncService.CalendarAdapters
{
    public interface ICalendar
    {
        Task UpdateAsync(List<Appointment> appointments);
        Task DeleteAppointmentAsync(string id);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetNearestAppointmentsAsync();
        Task<string> AddAppointmentAsync(Appointment appointment);

    }
}
