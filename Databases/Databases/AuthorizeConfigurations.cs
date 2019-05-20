using MongoDB.Bson;

namespace Databases
{
    public class AuthorizeConfigurations
    {
        public ObjectId Id;
        public string ClientId;
        public string ClientSecret;
        public string User;
        public string AccessToken;
        public string RefreshToken;

        public Calendars.AuthorizeConfigurations ToGoogle()
        {
            var googleAuthorizeConfigs = new Calendars.AuthorizeConfigurations
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                User = User
            };

            return googleAuthorizeConfigs;
        }
    }
}
