using System;
using Microsoft.AspNetCore.SignalR.Client;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpAPI.Consts;
using System.Threading.Tasks;
using Xamarin.Forms;
using NeighborHelpMobileClient.Properties;
using NeighborHelpMobileClient.Utils;

namespace NeighborHelpMobileClient.Services
{
    public class HubConnector
    {
        #region Fields

        private HubConnection hubConnection;

        public Action<string, string> NewMessageAction;

        private IConnectorProvider ConnectionProvider = DependencyService.Get<IConnectorProvider>();

        #endregion Fields

        #region Properties

        public HubConnection Connection => hubConnection;

        private string HostAddress => ConnectionProvider.GetServerUrl();

        private string AuthorizationToken => ConnectionProvider.GetToken()?.Token;

        #endregion Properties

        public HubConnector(double requestTimeout = DefaultSettings.RequestTimeout)
        {
            string fullChatPath = $"{HostAddress}{ChatHubConsts.Path}";

            hubConnection = new HubConnectionBuilder()
                .WithUrl(fullChatPath, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(AuthorizationToken);
                    options.CloseTimeout = TimeSpan.FromSeconds(requestTimeout);
                    options.HttpMessageHandlerFactory = _ => SslSertificateValidator.BuildHttpClientHandler();
                })
                .Build();

            hubConnection.On<string, string>(ChatHubConsts.ReceiveClientsMesage, (message, user) =>
            {
                SendLocalMessage(user, message);
            });

            hubConnection.On<string>(ChatHubConsts.NotifyClients, (message) =>
            {
                SendLocalMessage(string.Empty, message);
            });
        }

        public async Task Start()
        {
            await Connection.StartAsync();
        }

        public async Task Stop()
        {
            await Connection.StopAsync();
        }

        void SendLocalMessage(string user, string message)
        {
            NewMessageAction?.Invoke(user, message);
        }
    }
}
