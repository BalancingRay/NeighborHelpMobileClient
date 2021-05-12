using Microsoft.AspNetCore.SignalR.Client;
using NeighborHelpMobileClient.Models;
using System;
using System.Collections.ObjectModel;
using NeighborHelpAPI.Consts;
using System.Threading.Tasks;
using Xamarin.Forms;
using NeighborHelpMobileClient.Services;

namespace NeighborHelpMobileClient.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        HubConnector hubConnector;

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

        public ChatViewModel()
        {
            hubConnector = new HubConnector()
            {
                NewMessageAction = (user, message) => SendLocalMessage(user, message)
            };

            Messages = new ObservableCollection<MessageData>();
            IsBusy = false;
            IsConnected = false;

            hubConnector.Connection.Closed += async (error) =>
            {
                SendLocalMessage(string.Empty, "Подключение закрыто...");
                IsConnected = false;
                await Task.Delay(5000);
                await Connect();
            };
        }
        // подключение к чату
        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnector.Connection.StartAsync();
                SendLocalMessage(string.Empty, "Вы вошли в чат...");

                IsConnected = true;
            }
            catch (Exception ex)
            {
                SendLocalMessage(string.Empty, $"Ошибка подключения: {ex.Message}");
            }
        }

        // Отключение от чата
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnector.Connection.StopAsync();
            IsConnected = false;
            SendLocalMessage(string.Empty, "Вы покинули чат...");
        }

        // Отправка сообщения
        async Task SendMessageAsync()
        {
            try
            {
                IsBusy = true;
                await hubConnector.Connection.InvokeAsync(ChatHubConsts.SendMessage, Message);
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
        // Добавление сообщения
        private void SendLocalMessage(string user, string message)
        {
            Messages.Insert(0, new MessageData
            {
                Message = message,
                User = user
            });
        }
    }
}
