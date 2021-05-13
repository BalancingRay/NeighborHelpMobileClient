using System;
using Microsoft.AspNetCore.SignalR.Client;
using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpAPI.Consts;
using System.Threading.Tasks;
using Xamarin.Forms;
using NeighborHelpMobileClient.Properties;
using NeighborHelpMobileClient.Utils;
using NeighborHelpModels.ControllersModel;

namespace NeighborHelpMobileClient.Services
{
    public class HubConnector
    {
        #region Fields

        private bool isStoppedManually = false;
        private int reconnectionTime = 5000;

        private HubConnection hubConnection;

        private IConnectorProvider ConnectionProvider = DependencyService.Get<IConnectorProvider>();

        public bool ShowSystemMessage = true;

        public bool EnableReconnecting = true;

        public Action<string, string> NewMessageAction;

        public Action<string> UpdateUserAction;

        public Action<bool> UpdateConnectedStateAction;

        #endregion Fields

        #region Properties

        public HubConnection Connection => hubConnection;

        private string HostAddress => ConnectionProvider.GetServerUrl();

        private string AuthorizationToken => ConnectionProvider.GetToken()?.Token;

        private bool isConnected;
        private bool IsConnected 
        {
           get  => isConnected;
            set
            {
                isConnected = value;
                UpdateConnectedStateAction?.Invoke(value);
            }
        }

        #endregion Properties


        #region Constructor

        public HubConnector(double requestTimeout = DefaultSettings.RequestTimeout)
        {
            string fullChatPath = $"{HostAddress}{ChatHubConsts.Path}";

            hubConnection = new HubConnectionBuilder()
                .WithUrl(fullChatPath, options =>
                {
                    options.AccessTokenProvider = () => Task.Run(() => AuthorizationToken);
                    options.CloseTimeout = TimeSpan.FromSeconds(requestTimeout);
                    options.HttpMessageHandlerFactory = _ => SslSertificateValidator.BuildHttpClientHandler();
                })
                .Build();

            hubConnection.On<string, string>(ChatHubConsts.ReceiveClientsMesage,
                (message, user) => SendLocalMessage(user, message));

            hubConnection.On<string>(ChatHubConsts.NotifyClients,
                (message) => SendLocalSystemMessage(message));

            hubConnection.Closed += async (error) => await OnConnectionClosed(error);

            ConnectionProvider.AddUpdateTokenCallback(OnNewUserLogin);
        }

        #endregion Constructor

        #region Public Methods
        public async Task SendMessageToServer(string message)
        {
            await Connection.InvokeAsync(ChatHubConsts.SendMessage, message);
        }

        public async Task Start()
        {
            isStoppedManually = false;

            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                IsConnected = true;

                SendLocalSystemMessage("The chat is started...");
            }
            catch (Exception ex)
            {
                SendLocalSystemMessage($"Connection error: {ex.Message}");
            }
        }

        public async Task Stop()
        {
            isStoppedManually = true;

            if (!IsConnected)
                return;

            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalSystemMessage("The chat is stopped...");
        }

        #endregion Public Methods

        #region Private Methods

        private void SendLocalMessage(string user, string message)
        {
            NewMessageAction?.Invoke(user, message);
        }

        private void SendLocalSystemMessage(string message)
        {
            if (ShowSystemMessage)
            {
                NewMessageAction?.Invoke(string.Empty, message);
            }
        }

        private async Task OnConnectionClosed(Exception exc)
        {
            SendLocalSystemMessage("The connection is lose...");
            IsConnected = false;
            if (EnableReconnecting && !isStoppedManually)
            {
                SendLocalSystemMessage("Try to reconnect...");
                await Task.Delay(reconnectionTime);
                await Start();
            }
        }

        private void OnNewUserLogin(AuthentificateToken token)
        {
            UpdateUser(token);
        }

        private async Task UpdateUser(AuthentificateToken token)
        {
            bool previousState = IsConnected;

            if (IsConnected)
                await Stop();

            string userId = token?.UserId;
            UpdateUserAction?.Invoke(userId);
            bool isUserNotEmpty = !string.IsNullOrWhiteSpace(userId);

            if (isUserNotEmpty && previousState)
            {
                await Start();
            }
        }

        #endregion Private message
    }
}
