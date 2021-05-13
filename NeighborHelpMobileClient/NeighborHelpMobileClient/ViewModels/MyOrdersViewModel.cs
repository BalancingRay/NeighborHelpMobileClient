using NeighborHelpMobileClient.ViewModels.Base;
using NeighborHelpMobileClient.Views;
using NeighborHelpModels.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    public class MyOrdersViewModel : OrdersListViewModelBase
    {
        private Command addOrderCommand;
        public Command AddOrderCommand => addOrderCommand
            ?? (addOrderCommand = new Command(async () => await OpenNewOrderPage()));

        public MyOrdersViewModel()
        {
            Title = "My orders";
        }

        protected override async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await OrderStore.GetCurrentUserItemsAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
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
            string navigationString = $"{nameof(MyOrderDetailPage)}?{nameof(OrderDetailViewModel.ItemId)}={item.Id}";
            await Shell.Current.GoToAsync(navigationString);
        }

        private async Task OpenNewOrderPage()
        {
            string navigationString = nameof(NewOrderPage);
            await Shell.Current.GoToAsync(navigationString);
        }
    }
}