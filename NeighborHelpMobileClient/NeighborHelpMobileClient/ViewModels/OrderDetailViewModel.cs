using NeighborHelpMobileClient.ViewModels.Base;
using NeighborHelpMobileClient.Views;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class OrderDetailViewModel : OrderViewModelBase
    {
        public OrderDetailViewModel()
        {
            Title = "Order Details";
            IsItemCommandVisible = true;
            CommandName = "Edit";
        }

        protected override async void OnCommandExecute()
        {
            if (string.IsNullOrEmpty(ItemId))
                return;
            string navigationString = $"{nameof(EditOrderPage)}?{nameof(EditOrderViewModel.ItemId)}={ItemId}";
            await Shell.Current.GoToAsync(navigationString);
        }
    }
}
