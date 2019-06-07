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
            eventLog1 = new EventLog();

            AutoLog = false;

            if (!EventLog.SourceExists("SyncService"))
                EventLog.CreateEventSource("SyncService", "MyLog");

            eventLog1.Source = "SyncService";
            eventLog1.Log = "MyLog";
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

            var client = new MongoClient("mongodb+srv://newadmin:02072012@calendarcluster-tjsbr.gcp.mongodb.net/test?retryWrites=true");
            var database = client.GetDatabase("SyncConfigurations");
            var collection = database.GetCollection<Log>("History");

            var configuratons = Configurations.GetInstance();

            var timer = new Timer();
            timer.Elapsed += (sender, e) => OnTimer(timer, user, collection);
            timer.Interval = configuratons.Timer;
            timer.Enabled = true;

            
            EventLog.WriteEntry("SyncService","Started");
        }

        protected override void OnStop()
        {
        }

        public void OnTimer(Timer timer, string user, IMongoCollection<Log> logCollection)
        {
            timer.Stop();
            SyncController.Sync(user, logCollection).Wait();
            timer.Start();
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
