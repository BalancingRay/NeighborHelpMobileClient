using NeighborHelpModels.ControllersModel;
using System;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IConnectorProvider
    {
        AuthentificateToken GetToken();

        void AddUpdateTokenCallback(Action<AuthentificateToken> onTokenUpdateCallback);

        void UpdateToken(AuthentificateToken value);

        string GetServerUrl();

        void UpdateServerUrl(string value);
    }
}
