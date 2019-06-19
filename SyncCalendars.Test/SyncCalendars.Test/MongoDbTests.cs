using Synchronizer;
using SyncService;
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
        private static readonly MongoDbAdapter _db = new MongoDbAdapter(TestUser);

        private static async Task CleanTestUserDb()
        {
            var items = await _db.GetCalendarItems();

            foreach (var item in items)
                await _db.Remove(item);
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

            await _db.Add(syncItem);

            var items = await _db.GetCalendarItems();

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

            await _db.Add(oldItem);

            var expected = "googleTest1";
            var syncList = new List<MainSyncItem> {
                new MainSyncItem()
                {
                    GoogleId = expected,
                    OutlookId = "outlookTest1"
                }
            };

            await _db.Synchronize(syncList);

            var items = await _db.GetCalendarItems();

            Assert.Single(items);

            Assert.Equal(expected, items[0].GoogleId);
        }
    }
}
