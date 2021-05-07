using NeighborHelpMobileClient.Services.Contracts;
using NeighborHelpMobileClient.Views;
using NeighborHelpModels.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace NeighborHelpMobileClient.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class EditOrderViewModel : BaseViewModel
    {

        #region Fields

        private string id;
        private string product;
        private string productDescription;
        private string cost;
        private double doubleCost;
        private int authorId;
        private int selectedTypeIndex;
        private int selectedStatusIndex;

        #endregion Fields


        #region Properties
        public string Product
        {
            get => product;
            set => SetProperty(ref product, value);
        }
        public string ProductDescription
        {
            get => productDescription;
            set => SetProperty(ref productDescription, value);
        }
        public string Cost
        {
            get => cost;
            set => SetProperty(ref cost, value);
        }

        public string ItemId
        {
            get => id;
            set
            {
                string old = id;
                id = value;

                if (old != id)
                    LoadItemId(value);
            }
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public ObservableCollection<string> OrderTypes { get; set; }
        public ObservableCollection<string> OrderStatuses { get; set; }

        public int SelectedTypeIndex
        {
            get => selectedTypeIndex;
            set => SetProperty(ref selectedTypeIndex, value);
        }

        public int SelectedStatusIndex
        {
            get => selectedStatusIndex;
            set => SetProperty(ref selectedStatusIndex, value);
        }

        public IOrderStore OrderStore => DependencyService.Get<IOrderStore>();

        #endregion Properties


        #region Constructor

        public EditOrderViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            InitializeOrderValuesLists();
        }

        private void InitializeOrderValuesLists()
        {
            OrderTypes = new ObservableCollection<string>(NeighborHelpModels.Extentions.OrderExtention.GetAllTypes(null));
            OrderStatuses = new ObservableCollection<string>(NeighborHelpModels.Extentions.OrderExtention.GetAllStatuses(null));
        }

        #endregion Constructor


        #region Methods
        private bool ValidateSave()
        {
            bool isNotEmptyFields = !string.IsNullOrWhiteSpace(Product)
                && !string.IsNullOrWhiteSpace(ProductDescription)
                && !string.IsNullOrWhiteSpace(Cost)
                &&  OrderTypes.ElementAtOrDefault(SelectedTypeIndex) !=null
                &&  OrderStatuses.ElementAtOrDefault(SelectedStatusIndex) != null
                && !string.IsNullOrWhiteSpace(id);

            if (isNotEmptyFields && double.TryParse(Cost, out double result))
            {
                doubleCost = result;
            }
            else
            {
                return false;
            }

            return true;
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {

            Order newItem = new Order()
            {
                Id = int.Parse(this.id),
                AuthorId = this.authorId,
                Product = this.Product,
                ProductDescription = this.ProductDescription,
                Cost = this.doubleCost,
                OrderType = OrderTypes.ElementAtOrDefault(SelectedTypeIndex),
                Status = OrderStatuses.ElementAtOrDefault(SelectedStatusIndex)
            };

            await OrderStore.UpdateItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}");
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await OrderStore.GetItemAsync(itemId);
                Product = item.Product;
                ProductDescription = item.ProductDescription;
                Cost = item.Cost.ToString();
                authorId = item.AuthorId;

                SelectedTypeIndex = OrderTypes.IndexOf(item.OrderType);
                SelectedStatusIndex = OrderStatuses.IndexOf(item.Status);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }

            #endregion Methods
        }
    }
}
