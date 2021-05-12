using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.ViewModels.Base;
using NeighborHelpMobileClient.Views;
using NeighborHelpModels.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class OrdersViewModel : OrdersListViewModelBase
    {
        private Command addOrderCommand;
        public Command AddOrderCommand => addOrderCommand 
            ?? (addOrderCommand = new Command(async ()=> await OpenNewOrderPage()));

        public OrdersViewModel()
        {
            Title = "Browse";
        }

        protected override async Task ExecuteLoadItemsCommand()
        {
            try
            {
                IsBusy = true;
                Items.Clear();

                string userId = DependencyService.Get<IConnectorProvider>()?.GetToken()?.UserId;

                var items = await OrderStore.GetItemsAsync();

                foreach (var item in items)
                {
                    bool isNotCurrentUserOrder = item.AuthorId.ToString() != userId;
                    if (isNotCurrentUserOrder)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async void OnItemSelected(Order item)
        {
            if (item == null)
                return;
            // This will push the ItemDetailPage onto the navigation stack
            string navigationString = $"{nameof(OrderDetailPage)}?{nameof(UseOrderDetailViewModel.ItemId)}={item.Id}";
            await Shell.Current.GoToAsync(navigationString);
        }

        private async Task OpenNewOrderPage()
        {
            string navigationString = nameof(NewOrderPage);
            await Shell.Current.GoToAsync(navigationString);
        }
    }
}