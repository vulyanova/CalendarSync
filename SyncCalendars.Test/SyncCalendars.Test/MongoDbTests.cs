using SyncService.DbAdapters.MongoDbAdapter;
using System.Collections.Generic;
using System.Threading.Tasks;
using Synchronizer.Models;
using Xunit;

namespace SyncCalendars.Test
{
    public class MongoDbTests
    {
        private const string TestUser = "calendartests";
        private static readonly MongoDbAdapter Db = new MongoDbAdapter(TestUser);

        private static async Task CleanTestUserDb()
        {
            var items = await Db.GetCalendarItems();

            foreach (var item in items)
                await Db.Remove(item);
        }

        [Fact]
        public async Task MongoDb_AddConnections_SuccessfullAddition()
        {
            await CleanTestUserDb();

            var expected = "googleTest";

            var syncItem = new MainSyncItem()
            {
                GoogleId = expected,
                OutlookId = "outlookTest",
                TeamUpId = "teamUp"
            };

            await Db.Add(syncItem);

            var items = await Db.GetCalendarItems();

            Assert.Equal(expected, items[0].GoogleId);
        }


        [Fact]
        public async Task MongoDb_SynchronizeConnections_SuccessfullSync()
        {
            await CleanTestUserDb();

            var oldItem = new MainSyncItem()
            {
                GoogleId = "google",
                OutlookId = "outlook"
            };

            await Db.Add(oldItem);

            var expected = "googleTest1";
            var syncList = new List<MainSyncItem> {
                new MainSyncItem()
                {
                    GoogleId = expected,
                    OutlookId = "outlookTest1"
                }
            };

            await Db.Synchronize(syncList);

            var items = await Db.GetCalendarItems();

            Assert.Single(items);

            Assert.Equal(expected, items[0].GoogleId);
        }
    }
}
