using Synchronizer;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synchronizer.Models;

namespace SyncService.DbAdapters
{
    public interface IDbInteraction
    {
        Task Synchronize(List<MainSyncItem> syncAppointments);
    }
}
