namespace Synchronizer.Models
{
    public class MainSyncItem
    {
        public string GoogleId;
        public string OutlookId;
        public string TeamUpId;

        public void AddConnection(string id, CalendarType type)
        {
            switch (type)
            {
                case CalendarType.Google:
                    GoogleId = id;
                    break;
                case CalendarType.Outlook:
                    OutlookId = id;
                    break;
                case CalendarType.TeamUp:
                    TeamUpId = id;
                    break;
            }
        }
    }

    
}
