using Calendars;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SyncCalendars.Test
{
    public class ApiTests
    {
        private static readonly AuthorizeConfigurations Configs = new AuthorizeConfigurations
        {
            User = "calendarTestUser",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            AccessToken = "accessToken",
            RefreshToken = "refreshToken"
        };

        private const string Url = "https://localhost:5001/api/";

        private static async Task<HttpResponseMessage> GetApiResponseAsync(string path)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(Url + path);
           
            return response;
        }

        private static async Task PostApiResponseAsync(string path, AuthorizeConfigurations configs)
        {
            var client = new HttpClient();
            await client.PostAsJsonAsync(Url + path, configs);
        }

        private static async Task DeleteApiResponseAsync(string path, string user)
        {
            var client = new HttpClient();
            await client.DeleteAsync(Url + path + user);
        }

        [Fact]
        public async Task AuthorizeConfigs_AddConfigurations_SuccessfullAddition()
        {
            var url = "authorize/";

            var response = await GetApiResponseAsync(url);
            var list = await response.Content.ReadAsAsync<string[]>();

            await PostApiResponseAsync(url, Configs);
            response = await GetApiResponseAsync(url);
            var listWithAdded = await response.Content.ReadAsAsync<string[]>();

            await DeleteApiResponseAsync(url, Configs.User);
            response = await GetApiResponseAsync(url);
            var listWithoutAdded = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(list.Length, listWithAdded.Length-1);
            Assert.Equal(list.Length, listWithoutAdded.Length);
        }
    }
}
