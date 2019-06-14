using Synchronizer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyncService.CalendarAdapters
{
    public interface ICalendar
    {
        Task DeleteAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetNearestAppointmentsAsync();
        Task<string> AddAppointmentAsync(Appointment appointment);
    }
}
