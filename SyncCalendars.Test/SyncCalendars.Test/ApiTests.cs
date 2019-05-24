using Calendars;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SyncCalendars.Test
{
    public class ApiTests
    {
        private static readonly AuthorizeConfigurations _configs = new AuthorizeConfigurations
        {
            User = "calendarTestUser",
            ClientId = "clientId",
            ClientSecret = "clientSecret",
            AccessToken = "accessToken",
            RefreshToken = "refreshToken"
        };

        private static readonly string _url = "https://localhost:5001/api/";

        private static async Task<HttpResponseMessage> GetApiResponseAsync(string path)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_url + path);
           
            return response;
        }

        private static async Task<HttpResponseMessage> PostApiResponseAsync(string path, AuthorizeConfigurations configs)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync(_url + path, configs);

            return response;
        }

        private static async Task<HttpResponseMessage> DeleteApiResponseAsync(string path, string user)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync(_url + path + user);

            return response;
        }

        [Fact]
        public async Task AuthorizeConfigs_AddConfigurations_SuccessfullAddition()
        {
            var url = "authorize/";

            var response = await GetApiResponseAsync(url);
            var list = await response.Content.ReadAsAsync<string[]>();

            await PostApiResponseAsync(url, _configs);
            response = await GetApiResponseAsync(url);
            var listWithAdded = await response.Content.ReadAsAsync<string[]>();

            await DeleteApiResponseAsync(url, _configs.User);
            response = await GetApiResponseAsync(url);
            var listWithoutAdded = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(list.Length, listWithAdded.Length-1);
            Assert.Equal(list.Length, listWithoutAdded.Length);
        }
    }
}
