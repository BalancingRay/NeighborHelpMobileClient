using NeighborHelpMobileClient.Properties;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.ControllersModel;

namespace NeighborHelpMobileClient.Services
{
    public class AuthificationTokenProvider : IConnectorProvider
    {
        private AuthentificateToken token;
        private string address = DefaultSettings.LocalHostAddress;

        public string GetServerUrl()
        {
            return address;
        }

        public AuthentificateToken GetToken()
        {
            return token;
        }

        public void UpdateServerUrl(string value)
        {
            address = value;
        }

        public void UpdateToken(AuthentificateToken value)
        {
            token = value;
        }
    }
}
