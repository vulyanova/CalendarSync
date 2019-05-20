using Synchronizer;
using System.Collections.Generic;

namespace SyncService.DbAdapters
{
    public interface IDbInteraction
    {
        void Add(MainSyncItem syncAppointment);
        void Remove(MainSyncItem syncAppointment);
        List<MainSyncItem> GetCalendarItems();
        void Save();
    }
}
