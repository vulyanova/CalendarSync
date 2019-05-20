using System.ServiceProcess;

namespace SyncService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SyncService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
