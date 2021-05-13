using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpModels.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels.Base
{
    public abstract class OrdersListViewModelBase : BaseViewModel
    {
        protected IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        protected Order _selectedItem;
        public Order SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private ObservableCollection<Order> _items;
        public ObservableCollection<Order> Items => _items
            ?? (_items = new ObservableCollection<Order>());


        private Command<Order> _itemTappedCommand;
        public Command<Order> ItemTapped => _itemTappedCommand
            ?? (_itemTappedCommand = new Command<Order>(order => OnItemSelected(order)));

        private Command _loadItemsCommand;
        public Command LoadItemsCommand => _loadItemsCommand
            ?? (_loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand()));

        protected abstract void OnItemSelected(Order item);

        protected abstract Task ExecuteLoadItemsCommand();

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }
    }
}
