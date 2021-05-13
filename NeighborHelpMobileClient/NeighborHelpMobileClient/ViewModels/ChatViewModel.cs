using NeighborHelpMobileClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using NeighborHelpMobileClient.Services;

namespace NeighborHelpMobileClient.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        #region Fields

        HubConnector hubConnector;

        #endregion Fields

        #region Properties

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set => SetProperty(ref isConnected, value);
        }

        public ObservableCollection<MessageData> Messages { get; }

        private Command sendMessage;
        public Command SendMessageCommand => sendMessage
            ?? (sendMessage = new Command(async ()=> await SendMessageAsync(), () => IsConnected));

        #endregion Properties

        #region Constructor

        public ChatViewModel()
        {
            hubConnector = new HubConnector()
            {
                NewMessageAction = (user, message) => SendLocalMessage(user, message),
                UpdateUserAction = (userId) => OnNewUserLogin(userId),
                UpdateConnectedStateAction = (value)=> IsConnected=value,
                ShowSystemMessage = true,
                EnableReconnecting = true
            };

            Messages = new ObservableCollection<MessageData>();
            IsBusy = false;
            IsConnected = false;
        }

        #endregion Constructor

        #region Public Methods

        public async Task Connect()
        {
            await hubConnector.Start();             
        }

        public async Task Disconnect()
        {
            await hubConnector.Stop();
        }

        #endregion Public Methods

        #region Private Methods

        private async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return;
            try
            {
                IsBusy = true;
                await hubConnector.SendMessageToServer(Message);
                Message = string.Empty;
            }
            catch (Exception ex)
            {
                SendLocalMessage(string.Empty, $"Ошибка отправки: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnNewUserLogin(string userId)
        {
            Messages.Clear();
        }

        private void SendLocalMessage(string user, string message)
        {
            Messages.Insert(0, new MessageData
            {
                Message = message,
                User = user
            });
        }

        #endregion Private Methods
    }
}
