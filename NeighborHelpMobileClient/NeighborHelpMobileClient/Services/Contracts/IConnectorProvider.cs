using System;
using System.Collections.Generic;
using System.Text;

namespace NeighborHelpMobileClient.Services.Contracts
{
    public interface IConnectorProvider
    {
        string GetToken();

        void UpdateToken(string value);

        string GetServerUrl();

        void UpdateServerUrl(string value);
    }
}
