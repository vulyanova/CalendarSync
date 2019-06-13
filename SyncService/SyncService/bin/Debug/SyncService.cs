using MongoDB.Driver;
using System.Configuration;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace SyncService
{
    public partial class SyncService : ServiceBase
    {
        public SyncService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (args.Length!=0)
            {             
                config.AppSettings.Settings["User"].Value = args[0];
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");
            }

            var user = config.AppSettings.Settings["User"].Value;

            Configurations.GetConfigurations(user).Wait();

            var configuratons = Configurations.GetInstance();

            var timer = new Timer();
            timer.Elapsed += (sender, e) => OnTimer(timer, user);
            timer.Interval = configuratons.Timer;
            timer.Enabled = true;

            
            EventLog.WriteEntry("SyncService","Started");
        }

        protected override void OnStop()
        {
        }

        public void OnTimer(Timer timer, string user)
        {
            timer.Stop();
            SyncController.Sync(user).Wait();
            timer.Start();
        }
    }
}
