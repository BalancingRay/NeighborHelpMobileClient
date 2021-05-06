using NeighborHelpMobileClient.Properties;
using NeighborHelpMobileClient.Services.Contracts;

namespace NeighborHelpMobileClient.Services
{
    public class AuthificationTokenProvider : IConnectorProvider
    {
        private string token;
        private string address = DefaultSettings.LocalHostAddress;

        public string GetServerUrl()
        {
            return address;
        }

        public string GetToken()
        {
            return token;
        }

        public void UpdateServerUrl(string value)
        {
            address = value;
        }

        public void UpdateToken(string value)
        {
            token = value;
        }
    }
}
