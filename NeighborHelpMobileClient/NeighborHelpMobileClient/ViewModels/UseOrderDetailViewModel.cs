using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.ViewModels.Base;
using NeighborHelpModels.Models;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class UseOrderDetailViewModel : OrderViewModelBase
    {
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        public UseOrderDetailViewModel()
        {
            Title = "Order Details";
            IsItemCommandVisible = true;
            CommandName = "Apply";
        }

        protected override async void OnCommandExecute()
        {
            if (string.IsNullOrEmpty(ItemId)
                || !await ApplyUseOrderAction())
                return;

            if (await OrderStore.UseOrder(LoadedItem))
            {
                await LoadItemId(ItemId);
            }
            else
            {
                ErrorMessage = "Attempt failed";
            }
        }

        private async Task<bool> ApplyUseOrderAction()
        {
            string title = "Appply selected order?";
            string message = $"Do you want to {LoadedItem.OrderType} {LoadedItem.Product} with cost {LoadedItem.Cost}?";
            return await DependencyService.Get<IMessageBoxService>().ConfirmAction(title, message);
        }

        protected override async Task LoadItemId(string itemId)
        {
            await base.LoadItemId(itemId);

            if (LoadedItem == null)
            {
                IsItemCommandVisible = false;
            }
            else
            {
                string clientId = LoadedItem.ClientId.ToString();
                string currentUserId = DependencyService.Get<IConnectorProvider>().GetToken()?.UserId;
                bool isItemHasActiveState = Status == NeighborHelpModels.Models.Consts.OrderStatus.ACTIVE;

                IsItemCommandVisible = currentUserId != null
                    && currentUserId != clientId
                    && isItemHasActiveState;
            }
        }
    }
}
