using Synchronizer;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synchronizer.Models;

namespace SyncService.DbAdapters
{
    public interface IDbInteraction
    {
        Task Synchronize(List<MainSyncItem> syncAppointments);
        Task Add(MainSyncItem syncAppointment);
        Task Remove(MainSyncItem syncAppointment);
        Task<List<MainSyncItem>> GetCalendarItems();
        Task Save();
    }
}
