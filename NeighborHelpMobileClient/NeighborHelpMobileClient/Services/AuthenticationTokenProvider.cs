using NeighborHelpMobileClient.Properties;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.ControllersModel;
using System;

namespace NeighborHelpMobileClient.Services
{
    public class AuthificationTokenProvider : IConnectorProvider
    {
        private AuthentificateToken token;
        private string address = DefaultSettings.LocalHostAddress;

        private Action<AuthentificateToken> OnTokenUpdate;

        public string GetServerUrl()
        {
            return address;
        }

        public AuthentificateToken GetToken()
        {
            return token;
        }

        public void AddUpdateTokenCallback(Action<AuthentificateToken> onTokenUpdateCallback)
        {
            if (onTokenUpdateCallback != null)
            {
                OnTokenUpdate += onTokenUpdateCallback;
            }
        }

        public void UpdateServerUrl(string value)
        {
            address = value;
        }

        public void UpdateToken(AuthentificateToken value)
        {
            token = value;
            OnTokenUpdate?.Invoke(value);
        }
    }
}
