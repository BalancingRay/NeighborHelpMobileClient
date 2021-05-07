using NeighborHelpModels.ControllersModel;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IConnectorProvider
    {
        AuthentificateToken GetToken();

        void UpdateToken(AuthentificateToken value);

        string GetServerUrl();

        void UpdateServerUrl(string value);
    }
}
